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
    }

    void Start()
    {
        // 일단 공통 루트로 시작
        EventManager.Instance.PostNotification(Event_Type.eGameStart, this);
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eGameStart:
                StartCoroutine(IntroEvent());
                break;
        }
    }

    IEnumerator IntroEvent()
    {
        bool isComplete = false;
        StartCoroutine(commonEvent.DoEvent(() => isComplete = true));
        yield return new WaitUntil(() => isComplete);
    }

}
