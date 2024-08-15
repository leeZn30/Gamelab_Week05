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
    public bool isAvailable;

    public GameObject targetLocationObject;
    public GameObject targetEnemy;
    public int targetCount;
    public int currCount;


}