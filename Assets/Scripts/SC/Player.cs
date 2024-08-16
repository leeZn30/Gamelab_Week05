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

    // 특정 위치에 도달했을 때 이벤트 트리거
    public delegate void LocationReached(Vector3 location);
    public static event LocationReached OnLocationReached;

}

