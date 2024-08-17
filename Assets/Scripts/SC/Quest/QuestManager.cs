using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestSO> allQuests = new List<QuestSO>();
    private Dictionary<string, QuestSO> questDictionary;
    public List<QuestSO> clearedQuests = new List<QuestSO>();

    public List<QuestSO> activeQuests = new List<QuestSO>();

    public Transform questListParent;  // 퀘스트가 표시될 부모 객체

    [SerializeField] GameObject questTextPrefab;
    [SerializeField] List<GameObject> activeQuestTexts = new List<GameObject>();

    public int sumRevoltQuest = 0;

    void Start()
    {
        questDictionary = allQuests.ToDictionary(quest => quest.questName);
        foreach (var quests in allQuests)
        {
            quests.isActived = false;
            quests.isCompleted = false;
            if (quests.KillEnemies)
            {
                quests.currCount = 0;
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnQuestClear(string questName)
    {
        StartCoroutine(CompleteQuest(questName));
        switch (questName)
        {
            case "ClothQuest":
                if (FindQuest("ExtraClothQuest").isCompleted)
                {
                    AcceptQuest("NoteLastQuest");
                }
                break;
            case "ExtraClothQuest":
                if (FindQuest("ClothQuest").isCompleted)
                {
                    AcceptQuest("NoteLastQuest");
                }
                break;
            case "NoteLastQuest":
                Debug.Log("특정 위치 가기 퀘스트 클리어");
                // StartCoroutine(NoteRouteManager.Instance.noteEvent.CallEvent("FinalBattle"));
                EventManager.Instance.PostNotification(Event_Type.eNoteQuestDone, this, FindQuest(questName));
                break;
            case "RevoltQuest3":
                if (FindQuest("RevoltQuest4").isCompleted)
                {
                    questName = "RevoltQuest34";
                }
                break;
            case "RevoltQuest4":
                if (FindQuest("RevoltQuest3").isCompleted)
                {
                    questName = "RevoltQuest34";
                }
                break;

            case "RevoltQuest6":
                if (FindQuest("RevoltQuest7").isCompleted)
                {
                    questName = "RevoltQuest67";
                }
                break;
            case "RevoltQuest7":
                if (FindQuest("RevoltQuest6").isCompleted)
                {
                    questName = "RevoltQuest67";
                }
                break;
        }

        QuestSO quest = FindQuest(questName);

        if (!clearedQuests.Contains(quest))
        {
            clearedQuests.Add(quest);

            if (quest.eventType == Event_Type.eRevoltQuestDone)
            {
                sumRevoltQuest++;
                EventManager.Instance.PostNotification(quest.eventType, this, sumRevoltQuest);
            }

        }
    }

    public QuestSO FindQuest(string questName)
    {
        if (questDictionary.TryGetValue(questName, out QuestSO quest))
        {
            return quest;
        }

        return null;
    }

    public void AcceptQuest(string questName)
    {
        QuestSO quest = FindQuest(questName);
        if (!quest.isActived)
        {
            activeQuests.Add(quest);
            quest.isActived = true;
            GameObject newQuestText = Instantiate(questTextPrefab, questListParent);
            newQuestText.GetComponent<TextMeshProUGUI>().text = EditText(quest);
            activeQuestTexts.Add(newQuestText);

            if (quest.ReachLocation)
            {
                GameObject.Find(quest.targetObject).GetComponent<TargetLocation>().isActived = true;
                GameObject.Find(quest.targetObject).GetComponent<TargetLocation>().targetQuest = questName;
            }
            else if (quest.ReachItem)
            {
                GameObject.Find(quest.targetObject).GetComponent<ObjectInteraction>().isQuest = true;
                GameObject.Find(quest.targetObject).GetComponent<ObjectInteraction>().questName = questName;
            }
            else if (quest.ReachNPC)
            {
                GameObject.Find(quest.targetObject).GetComponent<NPCInteraction>().isActive = true;
                GameObject.Find(quest.targetObject).GetComponent<NPCInteraction>().targetQuest = questName;
            }
        }
    }

    public void UnderlineQuest(string questName)
    {
        for (int j = 0; j < activeQuests.Count; j++)
        {
            if (activeQuests[j].questName == questName)
            {
                activeQuests[j].isCompleted = true;
                GameObject questText = activeQuestTexts[j];
                questText.GetComponent<TextMeshProUGUI>().text = $"<s>{questText.GetComponent<TextMeshProUGUI>().text}</s>";
            }
        }
    }

    public IEnumerator CompleteQuest(string questName)
    {
        for (int j = 0; j < activeQuests.Count; j++)
        {
            if (activeQuests[j].questName == questName)
            {
                activeQuests[j].isCompleted = true;
                GameObject questText = activeQuestTexts[j];
                questText.GetComponent<TextMeshProUGUI>().text = $"<s>{questText.GetComponent<TextMeshProUGUI>().text}</s>";

                // 퀘스트 제거 (페이드 아웃 애니메이션)
                for (float i = 1; i >= 0; i -= Time.deltaTime)
                {
                    questText.GetComponent<TextMeshProUGUI>().color = new Color(questText.GetComponent<TextMeshProUGUI>().color.r, questText.GetComponent<TextMeshProUGUI>().color.g, questText.GetComponent<TextMeshProUGUI>().color.b, i);
                    yield return null;
                }
                Destroy(questText);
                activeQuests.RemoveAt(j);
                activeQuestTexts.RemoveAt(j);
            }
        }
    }

    public string EditText(QuestSO quest)
    {
        string tempText = quest.description;
        if (quest.KillEnemies)
        {
            tempText += " (" + quest.currCount + "/" + quest.targetCount + ")";
        }

        return tempText;
    }

    public void OnEnemyKilled(int enemyID)
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].KillEnemies && !activeQuests[i].isCompleted)
            {
                if (activeQuests[i].targetEnemyID == enemyID)
                {
                    activeQuests[i].currCount++;
                    activeQuestTexts[i].GetComponent<TextMeshProUGUI>().text = EditText(activeQuests[i]);
                    QuestManager tempQuestManager = GameObject.Find("QuestManager").GetComponent<QuestManager>(); ;
                    if (activeQuests[i].currCount == activeQuests[i].targetCount)
                    {
                        if (tempQuestManager.FindQuest(activeQuests[i].questName).returnNPC)
                        {
                            tempQuestManager.UnderlineQuest(activeQuests[i].questName);
                        }
                        else
                        {
                            tempQuestManager.OnQuestClear(activeQuests[i].questName);
                        }
                    }
                }
            }
        }
    }

    public bool checkQuest(string questname)
    {
        QuestSO quest = FindQuest(questname);
        if (quest.isCompleted)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}