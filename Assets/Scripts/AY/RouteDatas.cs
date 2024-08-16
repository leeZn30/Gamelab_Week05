
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
}
