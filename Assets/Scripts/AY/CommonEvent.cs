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
    [SerializeField] int introDialogueId0; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] int introDialogueId1; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] int introDialogueId2; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] int introDialogueId3; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] Note note0;
    [SerializeField] DoorInteraction RoomDoor;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public IEnumerator DoEvent(Action onComplete)
    {
        Debug.Log("Start Common Event");

        yield return StartCoroutine(Intro());

        onComplete?.Invoke();
        Debug.Log("End Common Event");
    }

    IEnumerator Intro()
    {
        DialogueManager.Instance.SetDialogueID(introDialogueId0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        note0.CallOpenNote();
        yield return new WaitUntil(() => !NoteRouteManager.Instance.noteUI.activeSelf);

        DialogueManager.Instance.SetDialogueID(introDialogueId1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);



    }


}
