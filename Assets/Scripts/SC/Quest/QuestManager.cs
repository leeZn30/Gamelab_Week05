using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestSO> activeQuests = new List<QuestSO>();  // 진행 중인 퀘스트 목록
    public List<QuestSO> completedQuests = new List<QuestSO>();  // 완료된 퀘스트 목록

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

            if (quest.mustReachLocation && quest.targetLocationObject != null)
            {
                quest.targetLocationObject.SetActive(true); // 목표 위치의 오브젝트 활성화
            }

            // 퀘스트 조건에 따라 델리게이트에 메소드 추가
            if (quest.mustKillEnemies)
            {
                Enemy.OnEnemyDeath += OnEnemyKilled;
            }
            if (quest.mustReachLocation)
            {
                quest.targetLocationObject.SetActive(true);
            }

            Debug.Log($"{quest.questName} 퀘스트 수락됨!");
        }
    }


    private void OnEnemyKilled(Enemy enemy)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.mustKillEnemies && !quest.isCompleted)
            {
                // 적이 죽었을 때 퀘스트 업데이트 코드
                quest.isCompleted = true; // 예시로 퀘스트 완료 처리
                completedQuests.Add(quest); // 완료된 퀘스트 목록에 추가
                activeQuests.Remove(quest); // 진행 중인 퀘스트 목록에서 제거
                Debug.Log($"{quest.questName} 퀘스트 완료!");
            }
        }
    }
}
