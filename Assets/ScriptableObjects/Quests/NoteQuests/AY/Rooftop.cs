using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooftop : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (QuestManager.Instance.activeQuests.Contains(QuestManager.Instance.FindQuest("NoteLastQuest"))
            &&
            QuestManager.Instance.activeQuests.Contains(QuestManager.Instance.FindQuest("RevoltLastQuest")))
            {
                // 최종 선택 시작
                CommonRouteManager.Instance.LastChoiceEventCheck();
            }
            else if (QuestManager.Instance.activeQuests.Contains(QuestManager.Instance.FindQuest("NoteLastQuest")))
            {
                NoteRouteManager.Instance.callFinalBattle();
            }
            else if (QuestManager.Instance.activeQuests.Contains(QuestManager.Instance.FindQuest("RevoltLastQuest")))
            {
                RevoltRouteManager.Instance.CallFinalEvent();
            }
        }
    }
}
