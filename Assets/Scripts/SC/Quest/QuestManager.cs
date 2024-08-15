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
            // 퀘스트 조건에 따라 델리게이트에 메소드 추가
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
                // 적이 죽었을 때 퀘스트 업데이트 코드
                quest.isCompleted = true; // 예시로 퀘스트 완료 처리
                completedQuests.Add(quest); // 완료된 퀘스트 목록에 추가
                activeQuests.Remove(quest); // 진행 중인 퀘스트 목록에서 제거
                Debug.Log($"{quest.questName} 퀘스트 완료!");
            }
        }
    }

    private void OnLocationReached(Vector3 location)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.mustReachLocation && !quest.isCompleted)
            {
                // 특정 위치에 도달했을 때 퀘스트 업데이트 코드
                quest.isCompleted = true; // 예시로 퀘스트 완료 처리
                Debug.Log($"{quest.questName} 퀘스트 완료!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 특정 게임 오브젝트에 들어갔는지 확인
        {
            foreach (var quest in activeQuests)
            {
                if (quest.mustReachLocation && !quest.isCompleted)
                {
                    // 특정 게임 오브젝트에 도달했을 때 퀘스트 업데이트 코드
                    quest.isCompleted = true; // 예시로 퀘스트 완료 처리
                    completedQuests.Add(quest); // 완료된 퀘스트 목록에 추가
                    activeQuests.Remove(quest); // 진행 중인 퀘스트 목록에서 제거
                    Debug.Log($"{quest.questName} 퀘스트 완료!");
                }
            }
        }
    }
}
