using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    public CSVReader csvReader;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public Dialogue GetDialogueById(int id)
    {
        foreach (Dialogue dialogue in csvReader.GetDialogues())
        {
            if (dialogue.id == id)
                return dialogue;
        }
        return null; // 해당 id를 가진 대사가 없으면 null 반환
    }
    public Dialogue[] GetDialogues(int someParameter, int lineY)
    {
        return csvReader.GetDialogues();
    }
}
