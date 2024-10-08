using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public QuestManager questManager;
    public QuestSO[] quest;

    void Start()
    {
        AcceptQuest(quest[0]);
        AcceptQuest(quest[1]);
    }

    public void AcceptQuest(QuestSO quest)
    {
        questManager.AcceptQuest(quest.questName);
    }
}

