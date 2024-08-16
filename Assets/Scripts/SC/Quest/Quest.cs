using UnityEngine;

public enum QuestName { Quest1, Quest2, Quest3 }

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class QuestSO : ScriptableObject
{
    public QuestName questName;
    public string description;
    public bool KillEnemies;
    public bool ReachLocation;
    public bool isCompleted;
    public bool isActived;
    public bool isAvailable;

    public string targetLocationObject;
    public int targetEnemyID;
    public int targetCount;
    public int currCount;
}