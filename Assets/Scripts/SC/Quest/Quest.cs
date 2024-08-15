using UnityEngine;

public enum QuestName { Quest1, Quest2, Quest3 } // ���������� ����Ʈ �̸� ����

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class QuestSO : ScriptableObject
{
    public QuestName questName;
    public string description;
    public bool mustKillEnemies;
    public bool mustReachLocation;
    public bool isCompleted;
    public bool isAvailable;

    public GameObject targetLocationObject; // ��ǥ ��ġ�� ���� ������Ʈ
}