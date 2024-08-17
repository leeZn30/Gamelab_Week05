using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Event_Type
{
    normal,
    eGameStart,
    eRevoltQuestDone,
    eNoteRead,
    eNoteQuestDone,
    eSave,
    eLoad
}

public interface IListener
{
    void OnEvent(Event_Type EventType, Component sender, object Param = null);
}
