
[System.Serializable]
public class NoteData
{
    public string noteID;
    public int order;
    public string content;
    public bool isTarget = false;

    public NoteData(string id, string text, int order)
    {
        noteID = id;
        this.order = order;
        content = text;
    }
}

[System.Serializable]
public class QuestData
{
    public string questID;
    public string description;
    public bool isStart = false;
    public bool isCompleted = false;

    public bool isKillQuest;
    public bool isReachQuest;
    public bool isItemQuest;

    public bool isActived;
    public bool isAvailable;

    public string targetLocationObject;
    public int targetEnemyID;
    public int targetCount;
    public int currCount;

    public QuestData(string id, string desc)
    {
        questID = id;
        description = desc;
    }

}

