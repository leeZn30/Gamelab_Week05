using System.Collections.Generic;
using UnityEngine;
using static Player;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestSO> activeQuests = new List<QuestSO>();
    public List<QuestSO> completedQuests = new List<QuestSO>();
    [SerializeField] QuestClear questClear;

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

            if (quest.ReachLocation && quest.targetLocationObject != null)
            {
                quest.targetLocationObject.SetActive(true);
            }

            Debug.Log($"{quest.questName} Äù½ºÆ® ¼ö¶ôµÊ!");
        }
    }

    public void OnEnemyKilled(int enemyID)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.KillEnemies && !quest.isCompleted)
            {
                if (quest.targetEnemy.GetComponent<Enemy>().EnemyID == enemyID)
                {
                    quest.currCount++;
                    if (quest.currCount == quest.targetCount)
                    {
                        quest.isCompleted = true;
                        questClear.OnQuestClear(quest.questName);
                    }
                }
            }
        }
    }

   
}
