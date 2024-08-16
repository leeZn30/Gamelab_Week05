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
                GameObject.Find("QuestManager").GetComponent<QuestManager>().OnQuestClear(targetQuest);
            }
        }
    }
}
