using UnityEngine;

public class QuestNPCInteraction : MonoBehaviour
{
    public int initialDialogueId = 3; // 처음 말을 걸 때 출력할 대사 ID
    public int incompleteQuestDialogueId = 4; // 퀘스트가 완료되지 않았을 때 출력할 대사 ID
    public int completedQuestDialogueId = 5; // 퀘스트가 완료된 후 출력할 대사 ID
    public int postCompletionDialogueId = 6; // 퀘스트가 완료된 후 항상 출력할 대사 ID

    public QuestSO quest;
    public GameObject interactionUI;
    public bool questDialogueCompleted = false;

    private bool isPlayerInRange = false;

    private bool isSend = false;

    void Start()
    {
        interactionUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            int dialogueId;
            isSend = true;

            if (questDialogueCompleted)
            {
                dialogueId = postCompletionDialogueId;
            }
            else if (!quest.isAvailable)
            {
                quest.isAvailable = true;
                dialogueId = initialDialogueId;
            }
            else if (!quest.isCompleted)
            {
                dialogueId = incompleteQuestDialogueId;
            }
            else
            {
                questDialogueCompleted = true;
                dialogueId = completedQuestDialogueId;
            }

            DialogueManager.Instance.SetDialogueID(dialogueId);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionUI.SetActive(true); // 플레이어가 범위 내에 들어오면 E 키 UI 표시
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUI.SetActive(false); // 플레이어가 범위 밖으로 나가면 E 키 UI 숨김
            isSend = false;
        }
    }
}
