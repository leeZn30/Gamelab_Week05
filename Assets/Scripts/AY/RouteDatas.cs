[System.Serializable]
public class NoteData
{
    public string noteID;
    public string content;
    public int order;

    public bool isTarget = false;

    public NoteData(string id, string text, int order)
    {
        noteID = id;
        content = text;
        this.order = order;
    }
}

[System.Serializable]
public class QuestData
{
    public string questID;
}
