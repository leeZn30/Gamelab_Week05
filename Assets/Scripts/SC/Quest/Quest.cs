using UnityEngine;

public enum QuestName { Quest1, Quest2, Quest3 } // 열거형으로 퀘스트 이름 정의

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class QuestSO : ScriptableObject
{
    public QuestName questName;
    public string description;
    public bool mustKillEnemies;
    public bool mustReachLocation;
    public bool isCompleted;
    public bool isAvailable;

    public string targetLocationObjectName; // 목표 위치의 게임 오브젝트 이름 또는 태그
}