using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestClear : MonoBehaviour
{
    public void btn1Clear()
    {
        GameObject.Find("QuestManager").GetComponent<QuestManager>().OnQuestClear("quest1");
    }
}
