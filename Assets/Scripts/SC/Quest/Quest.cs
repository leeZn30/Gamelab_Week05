using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class QuestSO : ScriptableObject
{
    public string questName;
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