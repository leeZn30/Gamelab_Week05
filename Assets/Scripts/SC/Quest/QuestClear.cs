using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestClear : MonoBehaviour
{
    public void OnQuestClear(QuestSO quest)
    {
        string questName = quest.questName;

        switch (questName)
        {
            case "quest1":
                Debug.Log("퀘스트 클리어");
                break;
        }
    }
}
