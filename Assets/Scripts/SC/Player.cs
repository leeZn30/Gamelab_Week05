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

    // Ư�� ��ġ�� �������� �� �̺�Ʈ Ʈ����
    public delegate void LocationReached(Vector3 location);
    public static event LocationReached OnLocationReached;

}

