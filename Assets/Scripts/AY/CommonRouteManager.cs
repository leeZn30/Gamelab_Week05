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

    IEnumerator LastEventCheck()
    {
        bool isComplete = false;
        StartCoroutine(commonEvent.DoLastChoiceEvent(() => isComplete = true));
        yield return new WaitUntil(() => isComplete);
    }

    public void LastChoiceEventCheck()
    {
        StartCoroutine(LastEventCheck());
    }
}
