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

    public List<QuestSO> activeQuests = new List<QuestSO>();
    public Transform questListParent;  // 퀘스트가 표시될 부모 객체
    [SerializeField] GameObject questTextPrefab;  // 퀘스트 텍스트 프리팹
    [SerializeField] List<GameObject> activeQuestTexts = new List<GameObject>();

    void Start()
    {
        questDictionary = allQuests.ToDictionary(quest => quest.questName);
        foreach (var quests in allQuests)
        {
            quests.isActived = false;
            quests.isCompleted = false;
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
                Debug.Log("신도복 퀘스트 클리어");
                if (FindQuest("ExtraClothQuest").isCompleted)
                {
                    Debug.Log("이후 퀘스트 실행");
                    AcceptQuest("Location");
                }
                break;
            case "ExtraClothQuest":
                Debug.Log("여벌의 신도복 퀘스트 클리어");
                if (FindQuest("ClothQuest").isCompleted)
                {
                    Debug.Log("이후 퀘스트 실행");
                    AcceptQuest("Location");
                }
                break;
            case "Location":
                Debug.Log("특정 위치 가기 퀘스트 클리어");
                StartCoroutine(NoteRouteManager.Instance.noteEvent.CallEvent("FinalBattle"));
                break;

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
        if (quest.isAvailable)
        {
            activeQuests.Add(quest);
            quest.isActived = true;
            GameObject newQuestText = Instantiate(questTextPrefab, questListParent);
            //TextMeshProUGUI newQuestText = Instantiate(questTextPrefab, questListParent).GetComponent<TextMeshProUGUI>();
            newQuestText.GetComponent<TextMeshProUGUI>().text = EditText(quest);
            activeQuestTexts.Add(newQuestText);

            if (quest.ReachLocation)
            {
                GameObject.Find(quest.targetLocationObject).GetComponent<BoxCollider2D>().enabled = true;
                GameObject.Find(quest.targetLocationObject).GetComponent<TargetLocation>().targetQuest = questName;
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
        foreach (var quest in activeQuests)
        {
            if (quest.KillEnemies && !quest.isCompleted)
            {
                if (quest.targetEnemyID == enemyID)
                {
                    quest.currCount++;
                    EditText(quest);
                    if (quest.currCount == quest.targetCount)
                    {
                        quest.isCompleted = true;
                        OnQuestClear(quest.questName);
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