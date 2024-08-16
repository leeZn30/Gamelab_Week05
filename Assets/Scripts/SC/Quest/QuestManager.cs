using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Player;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestSO> allQuests = new List<QuestSO>();
    public List<QuestSO> activeQuests = new List<QuestSO>();
    [SerializeField] QuestClear questClear;
    public Transform questListParent;  // 퀘스트가 표시될 부모 객체
    public TextMeshProUGUI questTextPrefab;  // 퀘스트 텍스트 프리팹
    private List<TextMeshProUGUI> activeQuestTexts = new List<TextMeshProUGUI>();

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

    public void AcceptQuest(QuestSO quest)
    {
        if (quest.isAvailable && !activeQuests.Contains(quest))
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
                        questClear.OnQuestClear(quest);
                    }
                }
            }
        }
    }

    public bool checkQuest(string questname)
    {
        foreach (var quests in allQuests)
        {
            if (quests.questName == questname)
            {
                if (quests.isCompleted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
        Debug.Log("퀘스트 못찾음");
    }
}

