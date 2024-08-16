using UnityEngine;

public class QuestNPCInteraction : MonoBehaviour
{
    public int initialDialogueId = 3;
    public int incompleteQuestDialogueId = 4;
    public int completedQuestDialogueId = 5;
    public int postCompletionDialogueId = 6;
    private bool isSend = false;

    public QuestSO quest;
    public GameObject interactionUI;
    public bool questDialogueCompleted = false;

    private bool isPlayerInRange = false;

    void Start()
    {
        interactionUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            int dialogueId = GetDialogueIdBasedOnQuestState();
            DialogueManager.Instance.SetDialogueID(dialogueId);
            isSend = true;


        }
    }

    private int GetDialogueIdBasedOnQuestState()
    {
        if (questDialogueCompleted)
        {
            return postCompletionDialogueId;
        }
        else if (!quest.isAvailable)
        {
            quest.isAvailable = true;
            QuestManager.Instance.AcceptQuest(quest.questName);
            return initialDialogueId;
        }
        else if (!quest.isCompleted)
        {
            return incompleteQuestDialogueId;
        }
        else
        {
            questDialogueCompleted = true;
            return completedQuestDialogueId;
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
            interactionUI.SetActive(false);
            isSend = false;
        }
    }
}
