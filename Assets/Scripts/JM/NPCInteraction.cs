using UnityEngine;
public struct NPCInteractionStatus
{
    public bool isSend;
    public bool isActive;

    public NPCInteractionStatus(bool saveSend, bool saveActive)
    {
        isSend = false;
        isActive = false;
    }
}

public class NPCInteraction : MonoBehaviour, IListener
{
    public int dialogueId; // 이 NPC가 출력할 기본 대사 ID
    public GameObject interactionUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)

    protected bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    protected bool isSend = false;

    public bool isActive = false;
    public string targetQuest;

    public bool isDeath = false;
    private int saveIndex = -1;

    void Start()
    {
        if (interactionUI != null)
        {

            interactionUI.SetActive(false);
        }
    }

    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    protected virtual void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            DialogueManager.Instance.SetDialogueID(dialogueId);
            if (isActive)
            {
                QuestManager tempQuestManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
                if (tempQuestManager.FindQuest(targetQuest).returnNPC)
                {
                    tempQuestManager.UnderlineQuest(targetQuest);
                }
                else
                {
                    tempQuestManager.OnQuestClear(targetQuest);
                }
            }
            isSend = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!GetComponent<NPCInfo>().isBattle)
            {
                isPlayerInRange = true;
                if (interactionUI != null)
                {
                    interactionUI.SetActive(true);
                }
            }
        }
        else if(GetComponent<NPCInfo>() != null)
        {
            isPlayerInRange = true;
            if (interactionUI != null)
            {
                interactionUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (interactionUI != null && interactionUI.activeSelf == true)
            {
                interactionUI.SetActive(false);
            }

            isSend = false;
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        if (!isDeath)
        {
            switch (EventType)
            {
                case Event_Type.eSave:
                    NPCInteractionStatus npcInteractionStatus = new NPCInteractionStatus(isSend, isActive);
                    SaveManager.Instance.saveNPCInteractionStatus.Add(npcInteractionStatus);
                    saveIndex = SaveManager.Instance.saveNPCInteractionStatus.Count - 1;
                    break;
                case Event_Type.eLoad:
                    isActive = SaveManager.Instance.saveNPCInteractionStatus[saveIndex].isActive;
                    isSend = SaveManager.Instance.saveNPCInteractionStatus[saveIndex].isSend;
                    isPlayerInRange = false;
                    if (interactionUI != null)
                    {
                        interactionUI.SetActive(false);
                    }
                    break;
            }
        }
    }
}
