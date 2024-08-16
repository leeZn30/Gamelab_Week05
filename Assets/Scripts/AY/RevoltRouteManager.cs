using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevoltRouteManager : Singleton<RevoltRouteManager>, IListener
{
    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        throw new System.NotImplementedException();
    }
}
