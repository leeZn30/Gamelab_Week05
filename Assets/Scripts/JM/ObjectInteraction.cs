using UnityEngine;
using System.Data.Common;

public struct ObjectInteractionStatus
{
    public bool isCollected;
    public bool isSend;

    public ObjectInteractionStatus(bool collected, bool send)
    {
        isCollected = collected;
        isSend = send;
    }
}

public class ObjectInteraction : MonoBehaviour, IListener
{
    public GameObject interactionUI;

    public int dialogueId; // 이 오브젝트가 출력할 기본 대사 ID
    public GameObject CollectedObject; // 이 오브젝트가 출력할 기본 대사 ID

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    private bool isSend = false;

    public bool isCollected = false;
    public bool isQuest = false;
    public bool notQuest = false;
    public string questName;
    public int saveIndex = -1;
    public bool isDeath = false;
    
    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            if (isQuest)
            {
                DialogueManager.Instance.SetDialogueID(dialogueId);
                isSend = true;
                if (CollectedObject != null)
                {
                    //Destroy(CollectedObject);
                    SaveManager.Instance.tempDestroyGameObjects.Add(CollectedObject);
                    CollectedObject.SetActive(false);
                    isCollected = true;
                }
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
            else if (notQuest)
            {
                DialogueManager.Instance.SetDialogueID(dialogueId);
                isSend = true;
                if (CollectedObject != null)
                {
                    //Destroy(CollectedObject);
                    SaveManager.Instance.tempDestroyGameObjects.Add(CollectedObject);
                    CollectedObject.SetActive(false);
                    isCollected = true;
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eSave:
                if (!isDeath)
                {
                    ObjectInteractionStatus saveItemsStatus = new ObjectInteractionStatus(isCollected, isSend);
                    SaveManager.Instance.saveItemStatus.Add(saveItemsStatus);
                    saveIndex = SaveManager.Instance.saveItemStatus.Count - 1;
                }
                break;
            case Event_Type.eLoad:
                if (!isDeath)
                {
                    if (SaveManager.Instance.saveItemStatus[saveIndex].isCollected)
                    {
                        isCollected = true;
                    }
                    else
                    {
                        isCollected = false;
                    }
                    if (SaveManager.Instance.saveItemStatus[saveIndex].isSend)
                    {
                        isSend = true;
                    }
                    else
                    {
                        isSend = false;
                    }
                }
                break;
        }
    }
}
