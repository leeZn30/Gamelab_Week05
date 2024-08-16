using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Player;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestSO> activeQuests = new List<QuestSO>();
    [SerializeField] QuestClear questClear;
    [SerializeField] private TMP_Text questText;

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
            OnQuestUI();

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

    private void OnQuestUI()
    {
        questText.text = "";
        foreach (var quest in activeQuests)
        {
            if (quest.isCompleted)
            {
                questText.text += "<s>";
            }
            questText.text += quest.description;
            if (quest.KillEnemies)
            {
                questText.text += " (" + quest.currCount + "/" + quest.targetCount + ")";
            }

            if (quest.isCompleted)
            {
                questText.text += "</s>";
            }

            questText.text += '\n';
        }
    }
}

