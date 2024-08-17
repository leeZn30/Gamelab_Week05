using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocation : MonoBehaviour
{
    public string targetQuest;
    public bool isActived = false;

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
}
