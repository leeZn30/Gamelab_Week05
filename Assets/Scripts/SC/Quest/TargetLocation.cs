using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocation : MonoBehaviour, IListener
{
    public string targetQuest;
    public bool isActived = false;
    public int saveIndex = -1;

    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && isActived)
        {
            if (collider.CompareTag("Player"))
            {
                isActived = true;
                GetComponent<BoxCollider2D>().enabled = false;
                QuestManager tempQuestManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
                    if (tempQuestManager.FindQuest(targetQuest).returnNPC)
                    {
                        tempQuestManager.UnderlineQuest(targetQuest);
                    }
                    else
                    {
                        tempQuestManager.OnQuestClear(targetQuest);
                    }
                
            }
        }
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
            switch (EventType)
            {
                case Event_Type.eSave:
                    if (isActived)
                    {
                        SaveManager.Instance.saveTargetLocation.Add(true);
                    }
                    else
                    {
                        SaveManager.Instance.saveTargetLocation.Add(false);
                    }
                    saveIndex = SaveManager.Instance.saveTargetLocation.Count - 1;
                break;
                case Event_Type.eLoad:
                    if (SaveManager.Instance.saveTargetLocation[saveIndex])
                    {
                        isActived = true;
                    }
                    else
                    {
                        isActived = false;
                    }
                break;
            }
    }
}
