using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocation : MonoBehaviour
{
    public string targetQuest;
    private bool isActived;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && !isActived)
        {
            if (collider.CompareTag("Player"))
            {
                isActived = true;
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
