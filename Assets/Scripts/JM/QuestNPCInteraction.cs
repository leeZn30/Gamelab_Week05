using UnityEngine;

public class QuestNPCInteraction : MonoBehaviour
{
    public int initialDialogueId = 3; // 처음 말을 걸 때 출력할 대사 ID
    public int incompleteQuestDialogueId = 4; // 퀘스트가 완료되지 않았을 때 출력할 대사 ID
    public int completedQuestDialogueId = 5; // 퀘스트가 완료된 후 출력할 대사 ID
    public int postCompletionDialogueId = 6; // 퀘스트가 완료된 후 항상 출력할 대사 ID

    public QuestSO quest; // NPC가 주는 퀘스트 ScriptableObject
    public GameObject interactionUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)
    public bool questDialogueCompleted = false; // 퀘스트 완료 후 대화가 한 번 완료되었는지 확인

    private bool isPlayerInRange = false; // 플레이어가 NPC 근처에 있는지 확인

    void Start()
    {
        interactionUI.SetActive(false); // 처음에는 E 키 UI를 숨깁니다.
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.instance.HandleQuestInteraction(this);
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
