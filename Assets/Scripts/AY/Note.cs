using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] NoteData noteData;

    void Awake()
    {
        // note
        noteData = NoteRouteManager.Instannce.noteDatas.Find(e => e.noteID == gameObject.name);
    }

    void Update()
    {
        // 노트 할성화
        gameObject.SetActive(noteData.isTarget);
    }

    void OnMouseDown()
    {
        // 읽은거 보여주기

        // 읽었다고 이벤트 보내기
        EventManager.Instannce.PostNotification(Event_Type.eNoteRead, this, noteData);
    }
}
