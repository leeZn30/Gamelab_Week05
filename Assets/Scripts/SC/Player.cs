using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public QuestManager questManager;

    public void AcceptQuest(QuestSO quest)
    {
        questManager.AcceptQuest(quest);
    }
}

