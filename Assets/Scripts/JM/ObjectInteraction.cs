using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject interactionUI;

    public int dialogueId; // 이 오브젝트가 출력할 기본 대사 ID
    public GameObject CollectedObject; // 이 오브젝트가 출력할 기본 대사 ID

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    private bool isSend = false;

    public bool isCollected = false;
    public bool isQuest = false;
    public string questName;


    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            DialogueManager.Instance.SetDialogueID(dialogueId);
            isSend = true;

            if (isQuest)
            {
                QuestManager tempQuestManager = QuestManager.Instance;
                if (tempQuestManager.FindQuest(questName).returnNPC)
                {
                    tempQuestManager.UnderlineQuest(questName);
                }
                else
                {
                    tempQuestManager.OnQuestClear(questName);
                }
            }

            if (CollectedObject != null)
            {
                Destroy(CollectedObject);
                isCollected = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUI.SetActive(true);
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }
}
