using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Player;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestSO> allQuests = new List<QuestSO>();
    private Dictionary<string, QuestSO> questDictionary;

    public List<QuestSO> activeQuests = new List<QuestSO>();
    public Transform questListParent;  // 퀘스트가 표시될 부모 객체
    private TextMeshProUGUI questTextPrefab;  // 퀘스트 텍스트 프리팹
    private List<TextMeshProUGUI> activeQuestTexts = new List<TextMeshProUGUI>();

    void Start()
    {
        questDictionary = allQuests.ToDictionary(quest => quest.questName);
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

    public QuestSO FindQuest(string questName)
    {
        if (questDictionary.TryGetValue(questName, out QuestSO quest))
        {
            return quest;
        }

        Debug.Log("퀘스트 없음");
        return null;
    }

    public void AcceptQuest(string questName)
    {
        QuestSO quest = FindQuest(questName); 
        if (quest.isAvailable)
        {
            activeQuests.Add(quest);
            quest.isActived = true;
            TextMeshProUGUI newQuestText = Instantiate(questTextPrefab, questListParent);
            newQuestText.text = quest.description;
            activeQuestTexts.Add(newQuestText);

            if (quest.ReachLocation)
            {
                GameObject tempGameObject = GameObject.Find(quest.targetLocationObject);
                if (tempGameObject != null)
                {
                    tempGameObject.SetActive(true);
                }
            }
        }
    }

    public void OnQuestClear(string questName)
    {
        StartCoroutine(CompleteQuest(questName));
        switch (questName)
        {
            case "quest1":
                Debug.Log("퀘스트 클리어");
                break;
        }
    }

    public IEnumerator CompleteQuest(string questName)
    {
        for(int j = 0; j< activeQuests.Count; j++){
            if (activeQuests[j].questName == questName)
            {
                activeQuests[j].isCompleted = true;
                TextMeshProUGUI questText = activeQuestTexts[j];
                questText.text = $"<s>{questText.text}</s>";

                // 퀘스트 제거 (페이드 아웃 애니메이션)
                for (float i = 1; i >= 0; i -= Time.deltaTime)
                {
                    questText.color = new Color(questText.color.r, questText.color.g, questText.color.b, i);
                    yield return null;
                }

                Destroy(questText.gameObject);
                activeQuestTexts.RemoveAt(j);
            }
        }
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