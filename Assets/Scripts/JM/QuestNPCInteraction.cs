using UnityEditor.PackageManager.Requests;
using UnityEngine;
public struct QuestNPCInteractionStatus
{
    public bool isSend;
    public bool questDialogueCompleted;

    public QuestNPCInteractionStatus(bool saveSend, bool saveQuestDialogueCompleted)
    {
        isSend = saveSend;
        questDialogueCompleted = saveQuestDialogueCompleted;
    }
}

public class QuestNPCInteraction : MonoBehaviour, IListener
{
    [Header("순서")]
    public int order;

    public int initialDialogueId = 3;
    public int incompleteQuestDialogueId = 4;
    public int completedQuestDialogueId = 5;
    public int postCompletionDialogueId = 6;
    private bool isSend = false;

    public QuestSO[] quest;
    public GameObject interactionUI;

    public GameObject questStart;
    public GameObject onQuest;
    public GameObject completeQuest;
    public bool questDialogueCompleted = false;

    private bool isPlayerInRange = false;
    public bool isDeath = false;
    public int saveIndex = -1;

    void Start()
    {
        interactionUI.SetActive(false);
    }

    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    void Update()
    {
        if (!questDialogueCompleted)
        {
            bool chk = true;
            for (int i = 0; i < quest.Length; i++)
            {
                if (!quest[i].isActived)
                {
                    questStart.SetActive(true);
                    chk = false;
                }
            }

            if (chk)
            {
                for (int i = 0; i < quest.Length; i++)
                {
                    if (!quest[i].isCompleted)
                    {
                        questStart.SetActive(false);
                        onQuest.SetActive(true);
                        chk = false;
                    }
                }
            }

            if (chk)
            {
                onQuest.SetActive(false);
                completeQuest.SetActive(true);
            }
        }
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
                tempID = postCompletionDialogueId;
            }
        }

        if (tempID == -1)
        {
            for (int i = 0; i < quest.Length; i++)
            {
                if (!quest[i].isActived)
                {
                    QuestManager.Instance.AcceptQuest(quest[i].questName);
                    tempID = initialDialogueId;
                    Debug.Log("퀘스트 받음 실행됨");
                }
            }
        }

        if (tempID == -1)
        {
            for (int i = 0; i < quest.Length; i++)
            {
                if (!quest[i].isCompleted)
                {
                    tempID = incompleteQuestDialogueId;
                }
            }
        }

        if (tempID == -1)
        {
            questDialogueCompleted = true;
            tempID = completedQuestDialogueId;
            for (int i = 0; i < quest.Length; i++)
            {
                QuestManager.Instance.OnQuestClear(quest[i].questName);
            }
            completeQuest.SetActive(false);
        }

        return tempID;

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

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        if (!isDeath)
        {
            switch (EventType)
            {
                case Event_Type.eSave:
                    QuestNPCInteractionStatus questNpcInteractionStatus = new QuestNPCInteractionStatus(isSend, questDialogueCompleted);
                    SaveManager.Instance.saveQuestNPCInteractionStatus.Add(questNpcInteractionStatus);
                    saveIndex = SaveManager.Instance.saveQuestNPCInteractionStatus.Count - 1;
                    break;
                case Event_Type.eLoad:
                    questDialogueCompleted = SaveManager.Instance.saveQuestNPCInteractionStatus[saveIndex].questDialogueCompleted;
                    isSend = SaveManager.Instance.saveQuestNPCInteractionStatus[saveIndex].isSend;
                    break;
            }
        }
    }
}
