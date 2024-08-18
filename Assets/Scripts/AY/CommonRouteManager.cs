using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // switch (EventType)
        // {
        //     case Event_Type.eGameStart:
        //         StartCoroutine(IntroEvent());
        //         break;


        //     case Event_Type.eGrandmaTalked:
        //         StartCoroutine(());
        //         break;
        // }

        StartCoroutine(EventCheck(EventType));
    }

    IEnumerator EventCheck(Event_Type eventType)
    {
        bool isComplete = false;
        StartCoroutine(commonEvent.DoEvent(() => isComplete = true, eventType));
        yield return new WaitUntil(() => isComplete);
    }

}
