using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRouteManager : Singleton<CommonRouteManager>, IListener
{
    [Header("EventId")]
    [SerializeField] int lastEventId = 0;

    [Header("Event1")]
    int introId;

    void Start()
    {
        // 일단 공통 루트로 시작

    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eCommonEvent:
                break;
        }
    }

}
