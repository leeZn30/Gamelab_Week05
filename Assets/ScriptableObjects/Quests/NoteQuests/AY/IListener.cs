using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Event_Type
{
    normal,
    eGameStart,
    eRevoltQuestDone,
    eRevoltLastQuestDone,
    eNoteRead,
    eNoteLastQuestDone,
    eSave,
    eLoad,
    eGrandmaTalked
}

public interface IListener
{
    void OnEvent(Event_Type EventType, Component sender, object Param = null);
}
