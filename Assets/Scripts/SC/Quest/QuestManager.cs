using System.Collections.Generic;
using UnityEngine;

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
        Player.OnLocationReached += OnLocationReached;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= OnEnemyKilled;
        Player.OnLocationReached -= OnLocationReached;
    }

    public void AcceptQuest(QuestSO quest)
    {
        if (quest.isAvailable && !activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            // ����Ʈ ���ǿ� ���� ��������Ʈ�� �޼ҵ� �߰�
            if (quest.mustKillEnemies)
            {
                Enemy.OnEnemyDeath += OnEnemyKilled;
            }
            if (quest.mustReachLocation)
            {
                Player.OnLocationReached += OnLocationReached;
            }
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

    private void OnLocationReached(Vector3 location)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.mustReachLocation && !quest.isCompleted)
            {
                // Ư�� ��ġ�� �������� �� ����Ʈ ������Ʈ �ڵ�
                quest.isCompleted = true; // ���÷� ����Ʈ �Ϸ� ó��
                Debug.Log($"{quest.questName} ����Ʈ �Ϸ�!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾ Ư�� ���� ������Ʈ�� ������ Ȯ��
        {
            foreach (var quest in activeQuests)
            {
                if (quest.mustReachLocation && !quest.isCompleted)
                {
                    // Ư�� ���� ������Ʈ�� �������� �� ����Ʈ ������Ʈ �ڵ�
                    quest.isCompleted = true; // ���÷� ����Ʈ �Ϸ� ó��
                    completedQuests.Add(quest); // �Ϸ�� ����Ʈ ��Ͽ� �߰�
                    activeQuests.Remove(quest); // ���� ���� ����Ʈ ��Ͽ��� ����
                    Debug.Log($"{quest.questName} ����Ʈ �Ϸ�!");
                }
            }
        }
    }
}
