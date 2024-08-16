using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevoltRouteManager : Singleton<RevoltRouteManager>, IListener
{
    void Awake()
    {
        // 씬 이동해도 삭제되면 안됨
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        throw new System.NotImplementedException();
    }
}
