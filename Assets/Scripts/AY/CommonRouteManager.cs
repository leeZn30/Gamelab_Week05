using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonRouteManager : Singleton<CommonRouteManager>, IListener
{
    [Header("CommonEvent")]
    [SerializeField] CommonEvent commonEvent;


    void Awake()
    {
        // 이벤트 등록
        EventManager.Instance.AddListener(Event_Type.eGameStart, this);
        EventManager.Instance.AddListener(Event_Type.eGrandmaTalked, this);
    }

    void Start()
    {
        // 일단 공통 루트로 시작
        EventManager.Instance.PostNotification(Event_Type.eGameStart, this);
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        StartCoroutine(EventCheck(EventType));
    }

    IEnumerator EventCheck(Event_Type eventType)
    {
        bool isComplete = false;
        StartCoroutine(commonEvent.DoEvent(() => isComplete = true, eventType));
        yield return new WaitUntil(() => isComplete);
    }

    public void LastChoiceEventCheck()
    {
        if (NoteRouteManager.Instance.isLast && RevoltRouteManager.Instance.isLast)
        {
            // 최종 선택 칸
            Debug.Log("최종 선택!");

        }
        else if (NoteRouteManager.Instance.isLast && !RevoltRouteManager.Instance.isLast)
        {
            Debug.Log("노트 루트!");
            CallNoteFinal();
        }
        else if (!NoteRouteManager.Instance.isLast && RevoltRouteManager.Instance.isLast)
        {
            Debug.Log("반란 루트!");
            CallRevoltFinal();
        }
    }

    public void CallNoteFinal()
    {
        // 앞선 이벤트
        StartCoroutine(NoteRouteManager.Instance.CallNoteFinalEvent());
    }

    public void CallRevoltFinal()
    {
        // 앞선 이벤트
        StartCoroutine(RevoltRouteManager.Instance.CallRevoltFinalEvent());
    }

}
