using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public int dialogueId; // 이 오브젝트가 출력할 기본 대사 ID
    public GameObject openDoor; // 열린 상태의 도어 오브젝트
    public GameObject closedDoor; // 닫힌 상태의 도어 오브젝트
    public ObjectInteraction requiredObject; // 필요한 오브젝트 스크립트 참조

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    private bool isSend = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            if (requiredObject != null && requiredObject.isCollected)
            {
                // 필요한 오브젝트가 수집되었을 경우
                closedDoor.SetActive(false);
                openDoor.SetActive(true);
                isSend = true;
            }
            else
            {
                // 필요한 오브젝트가 수집되지 않았을 경우 대사 출력
                DialogueManager.Instance.SetDialogueID(dialogueId);
                isSend = true;
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
}
