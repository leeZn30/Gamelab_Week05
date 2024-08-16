/*using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public int dialogueId; // 대사 ID, 이 NPC가 말할 대사를 지정
    public GameObject interactionUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)

    private bool isPlayerInRange = false; // 플레이어가 NPC 근처에 있는지 확인
    private DialogueManager dialogueManager;

    void Start()
    {
        interactionUI.SetActive(false); // 처음에는 E 키 UI를 숨깁니다.
        dialogueManager = FindObjectOfType<DialogueManager>(); // DialogueManager 찾기
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {

            // 대화 진행 중인지 확인
            if (dialogueManager.isDialogueActive)
            {
                // 다음 대사로 넘기기
                dialogueManager.DisplayNextSentence();
            }
            else
            {
                // 대화 시작
                Dialogue dialogue = DatabaseManager.instance.GetDialogueById(dialogueId);
                if (dialogue != null)
                {
                    Time.timeScale = 0;

                    dialogueManager.StartDialogue(dialogue);
                }
            }
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
*/