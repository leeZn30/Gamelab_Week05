using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public List<QuestSO> savedQuest = new List<QuestSO>();
    public List<DoorInteraction> savedDoors = new List<DoorInteraction>();
    public List<ObjectInteractionStatus> saveItemsStatus = new List<ObjectInteractionStatus>();

    public List<GameObject> savedItems = new List<GameObject>();

    public NoteData savedNote;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Save()
    {
        if (savedItems.Count > 0)
        {
            GameObject saveItem = savedItems[savedItems.Count - 1];
            Destroy(saveItem);
            savedItems.RemoveAt(savedItems.Count - 1);
        }
        savedQuest.Clear();
        savedDoors.Clear();
        saveItemsStatus.Clear();
        EventManager.Instance.PostNotification(Event_Type.eSave, this);
    }

    public void Load()
    {
        Debug.Log("로드 실행");
        if (savedItems.Count > 0)
        {
            GameObject saveItem = savedItems[savedItems.Count - 1];
            saveItem.SetActive(true);
            savedItems.RemoveAt(savedItems.Count - 1);
        }
        EventManager.Instance.PostNotification(Event_Type.eLoad, this);
    }
}
