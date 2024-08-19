using System.Data.Common;
using UnityEngine;

public struct DoorInteractionStatus
{
    public bool isOpend;
    public DoorInteractionStatus(bool currOpend)
    {
        isOpend = currOpend;
    }
}

public class DoorInteraction : MonoBehaviour, IListener
{
    public Collider2D closedDoorCollider; // 닫힌 문에 붙어있는 콜라이더
    public int dialogueId; // 이 오브젝트가 출력할 기본 대사 ID
    public GameObject openDoor; // 열린 상태의 도어 오브젝트
    public GameObject closedDoor; // 닫힌 상태의 도어 오브젝트
    public ObjectInteraction KeyObject; // 필요한 오브젝트 스크립트 참조

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    private bool isSend = false;

    public bool isOpend = false;
    private int saveIndex = -1;
    public AudioClip doorSound; // 발생할 소리
    private AudioSource audioSource;
    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 만약 AudioSource가 없다면 자동으로 추가합니다.
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            if (KeyObject != null && KeyObject.isCollected)
            {
                //필요한 열쇠 수집 시
                OpenDoor();
            }
            else if (KeyObject != null && KeyObject.isCollected == false)
            {
                // 필요한 오브젝트가 수집되지 않았을 경우 대사 출력
                DialogueManager.Instance.SetDialogueID(dialogueId);
                isSend = true;
            }

            else if (KeyObject == null)
            {
                //열쇠 없을 시
                OpenDoor();
            }

            else
            {
                OpenDoor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isSend = false; // 다시 범위에 들어올 때 대사가 출력될 수 있도록 초기화
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }

    public void OpenDoor()
    {
        // 필요한 오브젝트가 없을 경우
        audioSource.PlayOneShot(doorSound);
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
        isSend = true;
        isOpend = true;

        if (closedDoorCollider != null)
            closedDoorCollider.enabled = false; // 닫힌 문 콜라이더 비활성화
    }

    public void CloseDoor()
    {
        audioSource.PlayOneShot(doorSound);
        if(closedDoor != null)
        {
            closedDoor.SetActive(false);
            closedDoor.SetActive(true);
        }

        if(openDoor != null)
        {
            openDoor.SetActive(true);
        }
        isOpend = false;
        if (closedDoorCollider != null)
            closedDoorCollider.enabled = true; // 닫힌 문 콜라이더 활성화
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eSave:
                DoorInteractionStatus doorInteractionStatus = new DoorInteractionStatus(isOpend);
                SaveManager.Instance.savedDoorStatus.Add(doorInteractionStatus);
                saveIndex = SaveManager.Instance.savedDoorStatus.Count - 1;
                break;
            case Event_Type.eLoad:
                if (saveIndex != -1)
                {
                    if (SaveManager.Instance.savedDoorStatus[saveIndex].isOpend)
                    {
                        OpenDoor();
                    }
                    else
                    {
                        CloseDoor();
                    }
                }
                break;
        }
    }
}
