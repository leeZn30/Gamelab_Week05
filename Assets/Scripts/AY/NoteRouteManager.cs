using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteRouteManager : Singleton<NoteRouteManager>, IListener
{
    [Header("노트 관련")]
    public TextAsset noteCSV;
    public List<NoteData> noteDatas = new List<NoteData>();
    public NoteData currentNoteData;
    [SerializeField] GameObject noteUI;
    [SerializeField] Button noteBtn;

    [Header("노트 이벤트")]
    [SerializeField] NoteEvent noteEvent;

    void Awake()
    {
        // 씬 이동해도 삭제되면 안됨
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        // else
        // {
        //     DestroyImmediate(gameObject);
        // }

        // 노트 CSV 읽고 노트 데이터 만들기
        List<Dictionary<string, object>> readCSV = CSVReader.Read(CSVReader.GetFilePath(noteCSV));
        foreach (Dictionary<string, object> item in readCSV)
        {
            NoteData data = new NoteData
            (
                item["NoteID"].ToString(),
                CSVReader.GetFormatLine(item["Content"].ToString()),
                (int)item["Order"]
            );

            noteDatas.Add(data);
        }
        currentNoteData = noteDatas[0];
        currentNoteData.isTarget = true;

        noteUI = GameObject.Find("NoteUI");
        noteBtn = noteUI.GetComponentInChildren<Button>();
        noteBtn.onClick.AddListener(() => CloseNote());
        noteUI.SetActive(false);

        // 이벤트 등록
        EventManager.Instance.AddListener(Event_Type.eNoteRead, this);

    }

    // 노트를 봤다면 이벤트 실행
    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eNoteRead:

                // 현재 노트 안보이게 하기
                currentNoteData.isTarget = false;

                // 이벤트 처리
                StartCoroutine(EventCheck());

                break;
        }
    }

    IEnumerator EventCheck()
    {
        bool isComplete = false;
        StartCoroutine(noteEvent.DoEvent(() => isComplete = true, currentNoteData.order));
        yield return new WaitUntil(() => isComplete);

        // 다음 노트 찾아서 보이게 하기
        NoteData nextNoteData = noteDatas.Find(e => e.order == currentNoteData.order + 1);
        if (nextNoteData != null)
        {
            currentNoteData = nextNoteData;
            currentNoteData.isTarget = true;
        }
    }

    public void OpenNote(string content)
    {
        noteUI.GetComponentInChildren<TextMeshProUGUI>().SetText(content);
        noteUI.SetActive(true); //노트 활성화

        Time.timeScale = 0;
    }

    public void CloseNote()
    {
        noteUI.SetActive(false); //노트 비활성화
        Time.timeScale = 1;
    }

}
