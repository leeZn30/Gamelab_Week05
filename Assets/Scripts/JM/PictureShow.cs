using System.Data.Common;
using UnityEngine;

public class PictureShow : MonoBehaviour
{
    public int dialogueId; // 이 오브젝트가 출력할 기본 대사 ID
    public GameObject PictureUI; // 열린 상태의 도어 오브젝트
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    private bool isShowing = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && !isShowing)
        {
            ShowPicture();
        }
        else if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive && isShowing)
        {
            ClosePicture();
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

    public void ShowPicture()
    {

        PictureUI.SetActive(true);
        Time.timeScale = 0;
        isShowing = true;
    }

    public void ClosePicture()
    {
        PictureUI.SetActive(false);
        Time.timeScale = 1;
        isShowing = false;
        DialogueManager.Instance.SetDialogueID(dialogueId);

    }
}
