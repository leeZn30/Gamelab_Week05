using UnityEngine;

public class ShowNote : MonoBehaviour
{
    public GameObject interactionUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)
    public GameObject NoteUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)

    private bool isPlayerInRange = false; // 플레이어가 NPC 근처에 있는지 확인
    private bool isNoteOpen = false; // 플레이어가 노트를 열었는가

    void Start()
    {
        interactionUI.SetActive(false); // 처음에는 E 키 UI를 숨깁니다.
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isNoteOpen)
        {
            OpenNote();
        }
        else if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && isNoteOpen)
        {
            CloseNote();
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


    void OpenNote()
    {
        NoteUI.SetActive(true); //노트 활성화
        Time.timeScale = 0;
        isNoteOpen = true;
    }

    void CloseNote()
    {
        NoteUI.SetActive(false); //노트 비활성화
        Time.timeScale = 1;
        isNoteOpen = false;
    }
}
