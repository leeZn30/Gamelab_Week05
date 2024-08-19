using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonEvent : MonoBehaviour
{
    public Coroutine currentEvent;
    public int lastChoice = -1;

    [Header("Player")]
    GameObject player;

    [Header("UI")]
    GameObject LastChoiceUI;
    Button noteButton;
    [SerializeField] Button revoltButton;

    [Header("Prefabs")]
    [SerializeField] GameObject resistanceDefault;

    [Header("Intro")]
    [SerializeField] int introDialogueId0; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] int introDialogueId1; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] int introDialogueId2; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] int introDialogueId3; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] Note note0;
    [SerializeField] DoorInteraction RoomDoor;

    [Header("Grandma")]
    [SerializeField] int dialogueIdG;
    [SerializeField] GameObject grandma;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");

        LastChoiceUI = GameObject.Find("LastChoiceUI");
        noteButton = GameObject.Find("NoteRouteButton").GetComponent<Button>();
        revoltButton = GameObject.Find("RevoltRouteButton").GetComponent<Button>();
        noteButton.onClick.AddListener(() => { lastChoice = 0; LastChoiceUI.SetActive(false); });
        revoltButton.onClick.AddListener(() => { lastChoice = 1; LastChoiceUI.SetActive(false); });
        LastChoiceUI.SetActive(false);
    }

    public IEnumerator DoEvent(Action onComplete, Event_Type type)
    {
        Debug.Log("Start Common Event");

        switch (type)
        {
            case Event_Type.eGameStart:
                yield return CommonRouteManager.Instance.currentEvent = StartCoroutine(Intro());
                break;

            case Event_Type.eGrandmaTalked:
                yield return CommonRouteManager.Instance.currentEvent = StartCoroutine(GrandmaEvent());
                break;
        }

        onComplete?.Invoke();
        Debug.Log("End Common Event");
    }

    public IEnumerator DoLastChoiceEvent(Action onComplete)
    {
        Debug.Log("Start Common Event");

        yield return LastChoiceEvent();

        onComplete?.Invoke();
        Debug.Log("End Common Event");
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(0.1f);

        DialogueManager.Instance.SetDialogueID(introDialogueId0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        note0.CallOpenNote();
        yield return new WaitUntil(() => !NoteRouteManager.Instance.noteUI.activeSelf);

        DialogueManager.Instance.SetDialogueID(introDialogueId1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        RoomDoor.OpenDoor();
        yield return new WaitForSeconds(0.5f);
        GameObject go = Instantiate(resistanceDefault, RoomDoor.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        DialogueManager.Instance.SetDialogueID(introDialogueId2);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        Destroy(go);
        yield return new WaitForSeconds(0.5f);

        DialogueManager.Instance.SetDialogueID(introDialogueId3);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);
    }

    IEnumerator GrandmaEvent()
    {
        // 할머니 말 끝날 때까지 기다리기
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 반란군 등장
        yield return new WaitForSeconds(0.5f);
        GameObject go1 = Instantiate(resistanceDefault, grandma.transform.position + Vector3.up, Quaternion.identity);
        SpriteRenderer sprite1 = go1.GetComponent<SpriteRenderer>();
        sprite1.flipX = true;
        yield return new WaitForSeconds(0.5f);
        GameObject go2 = Instantiate(resistanceDefault, grandma.transform.position + Vector3.right, Quaternion.identity);
        SpriteRenderer sprite2 = go2.GetComponent<SpriteRenderer>();
        sprite2.flipX = true;
        yield return new WaitForSeconds(0.5f);

        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueIdG);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // npc 나감
        SpriteRenderer sprite3 = grandma.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            // 투명도 조절
            float alpha = Mathf.Clamp01(1 - (elapsedTime / 1f));
            sprite1.color = new Color(sprite1.color.r, sprite1.color.g, sprite1.color.b, alpha);
            sprite2.color = new Color(sprite2.color.r, sprite2.color.g, sprite2.color.b, alpha);
            sprite3.color = new Color(sprite3.color.r, sprite3.color.g, sprite3.color.b, alpha);
            yield return null; // 다음 프레임까지 대기
        }
        // 페이드 아웃이 완료된 후 오브젝트를 비활성화 또는 삭제
        Destroy(go1);
        Destroy(go2);
        grandma.SetActive(false);
        sprite3.color = new Color(sprite3.color.r, sprite3.color.g, sprite3.color.b, 1);
    }

    IEnumerator LastChoiceEvent()
    {
        player.transform.position = GameObject.Find("RoopTopPosition").transform.position + Vector3.up * 4;
        yield return new WaitForSeconds(0.5f);

        // 대사

        // 최종 선택 하기
        LastChoiceUI.SetActive(true);
        yield return new WaitUntil(() => !LastChoiceUI.activeSelf);

        // 최종 선택 별 말
        if (lastChoice == 0)
        {
            // 말 시작
            NoteRouteManager.Instance.callFinalBattle();
        }
        else
        {
            // 말 시작

            // CommonRouteManager.Instance.CallRevoltFinal();
        }
    }


}
