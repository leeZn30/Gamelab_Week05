using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteEventManager : Singleton<RouteEventManager>, IListener
{
    Coroutine executedEvent;

    void Awake()
    {
        // 이벤트 등록
        EventManager.Instance.AddListener(Event_Type.eNoteRead, this);
        EventManager.Instance.AddListener(Event_Type.eCommonEvent, this);
        EventManager.Instance.AddListener(Event_Type.eRevoltQuestDone, this);
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eCommonEvent:
                break;

            case Event_Type.eNoteRead:
                break;

            case Event_Type.eRevoltQuestDone:
                break;
        }
    }

}
