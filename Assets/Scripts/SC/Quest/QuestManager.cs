using System.Collections.Generic;
using UnityEngine;
using static Player;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestSO> activeQuests = new List<QuestSO>();  // ���� ���� ����Ʈ ���
    public List<QuestSO> completedQuests = new List<QuestSO>();  // �Ϸ�� ����Ʈ ���

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

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += OnEnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= OnEnemyKilled;
    }

    public void AcceptQuest(QuestSO quest)
    {
        if (quest.isAvailable && !activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);

            if (quest.mustReachLocation && !string.IsNullOrEmpty(quest.targetLocationObjectName))
            {
                GameObject targetObject = GameObject.Find(quest.targetLocationObjectName);
                if (targetObject != null)
                {
                    targetObject.SetActive(true);
                }
            }

            if (quest.mustKillEnemies)
            {
                Enemy.OnEnemyDeath += OnEnemyKilled;
            }

            Debug.Log($"{quest.questName} ����Ʈ ������!");
        }
    }


    private void OnEnemyKilled(Enemy enemy)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.mustKillEnemies && !quest.isCompleted)
            {
                // ���� �׾��� �� ����Ʈ ������Ʈ �ڵ�
                quest.isCompleted = true; // ���÷� ����Ʈ �Ϸ� ó��
                completedQuests.Add(quest); // �Ϸ�� ����Ʈ ��Ͽ� �߰�
                activeQuests.Remove(quest); // ���� ���� ����Ʈ ��Ͽ��� ����
                Debug.Log($"{quest.questName} ����Ʈ �Ϸ�!");
            }
        }
    }
}
