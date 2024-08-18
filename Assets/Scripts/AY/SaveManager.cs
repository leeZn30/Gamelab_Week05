using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public List<QuestSO> savedQuest = new List<QuestSO>();
    public List<DoorInteraction> savedDoors = new List<DoorInteraction>();
    public List<ObjectInteraction> saveItemsScript = new List<ObjectInteraction>();

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
        saveItemsScript.Clear();
        EventManager.Instance.PostNotification(Event_Type.eSave, this);
    }

    public void Load()
    {
        if (savedItems.Count > 0)
        {
            GameObject saveItem = savedItems[savedItems.Count - 1];
            saveItem.SetActive(true);
            Debug.Log("오브젝트 퀘스트이름"+ saveItemsScript[savedItems.Count - 1].GetComponent<ObjectInteraction>().questName);
            savedItems.RemoveAt(savedItems.Count - 1);
        }
        EventManager.Instance.PostNotification(Event_Type.eLoad, this);
    }
}
