using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class QuestSO : ScriptableObject
{
    public string questName;
    public string description;
    public bool returnNPC;
    public bool KillEnemies;
    public bool ReachLocation;
    public bool ReachItem;
    public bool ReachNPC;
    public bool isCompleted;
    public bool isActived;
    //public bool isAvailable;
    public Event_Type eventType;

    public string targetObject;
    public int targetEnemyID;
    public int targetCount;
    public int currCount;
}