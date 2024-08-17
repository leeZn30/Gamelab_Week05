using UnityEngine;

public class SinkInteraction : MonoBehaviour
{
    public int dialogueId; // 이 오브젝트가 출력할 기본 대사 ID
    public GameObject wet; // 물로 젖는 효과를 나타내는 오브젝트

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    private bool isSend = false;

    void Update()
    {
        // 플레이어가 범위 내에 있고 E 키를 눌렀으며, 대화가 진행 중이 아니고 아직 대사가 출력되지 않은 경우
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isSend)
        {
            Wet();
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

    public void Wet()
    {
        wet.SetActive(false); // 물로 젖는 효과를 활성화
        isSend = true;
    }
}
