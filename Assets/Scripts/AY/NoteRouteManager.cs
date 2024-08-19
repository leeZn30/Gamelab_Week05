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
    public GameObject noteUI;
    [SerializeField] Button noteBtn;

    [Header("퀘스트 관련")]
    QuestSO currentQuestSO;

    [Header("노트 이벤트")]
    [SerializeField] NoteEvent noteEvent;

    [Header("최종 루트 진입 직전")]
    public bool isLast;

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
        currentNoteData = new NoteData(noteDatas[0].noteID, noteDatas[0].content, noteDatas[0].order);
        noteDatas[0].isTarget = true;

        noteUI = GameObject.Find("NoteUI");
        noteBtn = noteUI.GetComponentInChildren<Button>();
        noteBtn.onClick.AddListener(() => CloseNote());
        noteUI.SetActive(false);

        // 이벤트 등록
        EventManager.Instance.AddListener(Event_Type.eNoteRead, this);
        EventManager.Instance.AddListener(Event_Type.eNoteLastQuestDone, this);
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    // 노트를 봤다면 이벤트 실행
    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eNoteRead:

                // 현재 노트 안보이게 하기
                currentNoteData.isTarget = false;

                // 노트 닫을 때까지 기다림
                StartCoroutine(OnNoteClosed());

                break;

            case Event_Type.eNoteLastQuestDone:
                isLast = true;
                CommonRouteManager.Instance.LastChoiceEventCheck();
                break;

            case Event_Type.eSave:
                SaveManager.Instance.savedNote = new NoteData(currentNoteData.noteID, currentNoteData.content, currentNoteData.order);
                break;

            case Event_Type.eLoad:
                for (int i = 0; i < noteDatas.Count; i++)
                {
                    noteDatas[i].isTarget = false;
                }
                noteDatas[SaveManager.Instance.savedNote.order].isTarget = true;
                currentNoteData.order = SaveManager.Instance.savedNote.order;
                break;
        }
    }

    public IEnumerator CallNoteFinalEvent()
    {
        bool isComplete = false;
        StartCoroutine(noteEvent.DoEvent(() => isComplete = true, "NoteLastQuest"));
        yield return new WaitUntil(() => isComplete);
    }

    IEnumerator OnNoteClosed()
    {
        yield return new WaitUntil(() => !noteUI.activeSelf);

        StartCoroutine(NoteReadEventCheck());
    }

    IEnumerator NoteReadEventCheck()
    {
        bool isComplete = false;
        StartCoroutine(noteEvent.DoEvent(() => isComplete = true, currentNoteData.order));
        noteDatas.Find(e => e.order == currentNoteData.order).isTarget = false;
        yield return new WaitUntil(() => isComplete);

        // 다음 노트 찾아서 보이게 하기
        NoteData nextNoteData = noteDatas.Find(e => e.order == currentNoteData.order + 1);
        if (nextNoteData != null)
        {
            currentNoteData = new NoteData(nextNoteData.noteID, nextNoteData.content, nextNoteData.order);
            nextNoteData.isTarget = true;
        }
    }

    IEnumerator NoteQuestEventCheck()
    {
        bool isComplete = false;
        StartCoroutine(noteEvent.DoEvent(() => isComplete = true, currentQuestSO.questName));
        yield return new WaitUntil(() => isComplete);
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
