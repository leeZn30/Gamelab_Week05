using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public int dialogueId; // 이 오브젝트가 출력할 기본 대사 ID
    public GameObject CollectedObject; // 이 오브젝트가 출력할 기본 대사 ID

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    private bool isSend = false;

    public bool isCollected = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            DialogueManager.Instance.SetDialogueID(dialogueId);
            isSend = true;

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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }
}
