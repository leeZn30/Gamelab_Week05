using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public int dialogueId; // 이 NPC가 출력할 기본 대사 ID
    public GameObject interactionUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)

    private bool isPlayerInRange = false; // 플레이어가 NPC 근처에 있는지 확인

    void Start()
    {
        interactionUI.SetActive(false); // 처음에는 E 키 UI를 숨깁니다.
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.instance.HandleInteraction(this);
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
        }
    }
}
