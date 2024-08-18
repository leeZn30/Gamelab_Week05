using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public List<DoorInteractionStatus> savedDoorStatus = new List<DoorInteractionStatus>();
    public List<ObjectInteractionStatus> saveItemStatus = new List<ObjectInteractionStatus>();
    public List<NPCInfoStatus> saveNpcInfoStatus = new List<NPCInfoStatus>();
    public List<NPCInteractionStatus> saveNPCInteractionStatus = new List<NPCInteractionStatus>();
    public List<QuestNPCInteractionStatus> saveQuestNPCInteractionStatus = new List<QuestNPCInteractionStatus>();
    public List<QuestManagerStatus> saveQuestManagerStatus = new List<QuestManagerStatus>();



    public List<GameObject> tempDestroyGameObjects = new List<GameObject>();
    public List<GameObject> tempNPCDestroy = new List<GameObject>();

    public NoteData savedNote;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Save()
    {
        while (tempDestroyGameObjects.Count > 0)
        {
            GameObject saveItem = tempDestroyGameObjects[tempDestroyGameObjects.Count - 1];
            saveItem.GetComponent<ObjectInteraction>().isDeath = true;
            Destroy(saveItem);
            tempDestroyGameObjects.RemoveAt(tempDestroyGameObjects.Count - 1);
        }

        while (tempNPCDestroy.Count > 0)
        {
            GameObject saveItem = tempNPCDestroy[tempNPCDestroy.Count - 1];
            saveItem.GetComponent<NPCInfo>().isDeath = true;
            Destroy(saveItem);
            tempNPCDestroy.RemoveAt(tempNPCDestroy.Count - 1);
        }

        savedDoorStatus.Clear();
        saveItemStatus.Clear();
        saveNpcInfoStatus.Clear();
        saveNPCInteractionStatus.Clear();
        saveQuestNPCInteractionStatus.Clear();

        EventManager.Instance.PostNotification(Event_Type.eSave, this);
    }

    public void Load()
    {
        while (tempDestroyGameObjects.Count > 0)
        {
            GameObject saveItem = tempDestroyGameObjects[tempDestroyGameObjects.Count - 1];
            saveItem.SetActive(true);
            tempDestroyGameObjects.RemoveAt(tempDestroyGameObjects.Count - 1);
        }

        while (tempNPCDestroy.Count > 0)
        {
            GameObject saveItem = tempNPCDestroy[tempNPCDestroy.Count - 1];
            saveItem.SetActive(true);
            tempNPCDestroy.RemoveAt(tempNPCDestroy.Count - 1);
        }

        EventManager.Instance.PostNotification(Event_Type.eLoad, this);
    }
}