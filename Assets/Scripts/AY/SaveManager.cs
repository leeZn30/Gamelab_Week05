using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public List<QuestSO> savedQuest = new List<QuestSO>();
    public List<DoorInteraction> savedDoors = new List<DoorInteraction>();
    public List<ObjectInteraction> savedItems = new List<ObjectInteraction>();
    public NoteData savedNote;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Save()
    {
        savedQuest.Clear();
        savedDoors.Clear();
        savedItems.Clear();
        EventManager.Instance.PostNotification(Event_Type.eSave, this);
    }

    public void Load()
    {

    }
}
