using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEvent : MonoBehaviour
{
    public int currentEvent;

    [Header("Player")]
    GameObject player;

    [Header("Intro")]
    public int introDialogueId; // 대사 ID, 이 NPC가 말할 대사를 지정


    public IEnumerator DoEvent(Action onComplete, int eventNum)
    {
        Debug.Log("Start Common Event");

        switch (eventNum)
        {
            case 0:
                yield return StartCoroutine(Intro());
                break;

        }

        yield return null;

        onComplete?.Invoke();
        Debug.Log("End Common Event");
    }

    IEnumerator Intro()
    {
        DialogueManager.Instance.SetDialogueID(introDialogueId);
        yield return new WaitUntil(() => DialogueManager.Instance.isDialogueActive);
    }


}
