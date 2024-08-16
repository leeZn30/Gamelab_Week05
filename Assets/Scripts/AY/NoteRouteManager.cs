using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteRouteManager : Singleton<NoteRouteManager>, IListener
{
    [Header("노트 데이터")]
    public TextAsset noteCSV;
    public List<NoteData> noteDatas = new List<NoteData>();
    public NoteData currentNoteData;

    void Awake()
    {
        // 씬 이동해도 삭제되면 안됨
        if (Instannce == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        // 노트 CSV 읽고 노트 데이터 만들기

        // 이벤트 등록
        EventManager.Instannce.AddListener(Event_Type.eNoteRead, this);

    }


    // 노트를 봤다면 이벤트 실행
    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eNoteRead:

                // 현재 노트 안보이게 하기
                currentNoteData.isTarget = false;

                // 다음 노트 찾아서 보이게 하기
                currentNoteData = noteDatas.Find(e => e.order == currentNoteData.order + 1);
                currentNoteData.isTarget = true;
                break;
        }
    }
}
