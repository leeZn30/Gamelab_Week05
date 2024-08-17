using UnityEngine;

public class QuestNPCInteraction : MonoBehaviour
{
    public int initialDialogueId = 3;
    public int incompleteQuestDialogueId = 4;
    public int completedQuestDialogueId = 5;
    public int postCompletionDialogueId = 6;
    private bool isSend = false;

    public QuestSO[] quest;
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
        int tempID = -1;

        if (questDialogueCompleted)
        {
            if (tempID == -1)
            {
                tempID = 0;
            }
        }

        if (tempID == -1)
        {
            for (int i = 0; i < quest.Length; i++)
            {
                if (!quest[i].isActived)
                {
                    quest[i].isActived = true;
                    QuestManager.Instance.AcceptQuest(quest[i].questName);
                    tempID = 1;
                }
            }
        }

        if (tempID == -1)
        {
            for (int i = 0; i < quest.Length; i++)
            {
                if (!quest[i].isCompleted)
                {
                    tempID = 2;
                }
            }
        }

        if (tempID == -1)
        {
            for (int i = 0; i < quest.Length; i++)
            {
                if (quest[i].isCompleted)
                {
                    questDialogueCompleted = true;
                    tempID = 3;
                }
            }
        }

        /*
        if (questDialogueCompleted)
        {
            return postCompletionDialogueId;
        }else if (!quest.isAvailable)
            {
                quest.isAvailable = true;
                QuestManager.Instance.AcceptQuest(quest.questName);
                return initialDialogueId;
            }
        }else if (!quest.isCompleted)
        {
            return incompleteQuestDialogueId;
        }
        else if(quest.isCompleted)
        {
            questDialogueCompleted = true;
            return completedQuestDialogueId;
        }
        */

        switch (tempID)
        {
            case 0:
                return postCompletionDialogueId;
            case 1:
                return initialDialogueId;
            case 2:
                return incompleteQuestDialogueId;
            case 3:
                return completedQuestDialogueId;
            default:
                Debug.Log("퀘스트 NPC대사 오류");
                return -1;
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
