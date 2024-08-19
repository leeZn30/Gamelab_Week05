using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("노트 데이터")]
    [SerializeField] NoteData noteData;
    [SerializeField] GameObject noteBody;

    [Header("기타")]
    [SerializeField] GameObject interactionUI; // E 키 UI (플레이어의 자식 오브젝트로 설정된 UI)
    private bool isPlayerInRange = false; // 플레이어가 NPC 근처에 있는지 확인
    public bool isEnd = false;

    void Start()
    {
        // note
        noteData = NoteRouteManager.Instance.noteDatas.Find(e => e.noteID == gameObject.name);
        interactionUI.SetActive(false);
    }

    void Update()
    {
        noteBody.SetActive(noteData.isTarget);

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CallOpenNote();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && noteBody.activeSelf)
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

    public void CallOpenNote()
    {

        NoteRouteManager.Instance.OpenNote(noteData.content);

        // 이벤트 호출
        EventManager.Instance.PostNotification(Event_Type.eNoteRead, this, noteData);

        // 삭제
        //Destroy(gameObject);
    }

}
