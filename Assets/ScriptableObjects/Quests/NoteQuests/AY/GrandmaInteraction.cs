using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaInteraction : NPCInteraction
{
    protected override void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            DialogueManager.Instance.SetDialogueID(dialogueId);
            if (isActive)
            {
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
            isSend = true;

            // 할머니 이벤트 호출
            EventManager.Instance.PostNotification(Event_Type.eGrandmaTalked, this);
        }
    }
}
