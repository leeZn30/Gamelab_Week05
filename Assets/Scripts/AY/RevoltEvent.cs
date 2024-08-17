using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RevoltEvent : MonoBehaviour
{
    [Header("Player")]
    GameObject player;

    [Header("프리팹")]
    [SerializeField] GameObject resistance;
    [SerializeField] GameObject cultEnemy;
    [SerializeField] GameObject resistaceBoss;

    [Header("Event2")]
    [SerializeField] int dialogueId1;

    [Header("Event3")]
    [SerializeField] int dialogueId3_0;
    [SerializeField] int dialogueId3_1;
    [SerializeField] GameObject event2Door;

    [Header("Event4")]
    [SerializeField] int dialogueId4;

    [Header("Event5")]
    [SerializeField] int dialogueId5;
    [SerializeField] QuestSO questSO;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public IEnumerator DoEvent(Action onComplete, QuestSO quest)
    {
        Debug.Log("Start Revolt Event");

        switch (quest.questName)
        {
            case "JoiningQuest":
                break;

            case "RevoltQeust1":
                break;

            case "RevoltQuest2":
                yield return StartCoroutine(Event2());
                break;

            case "RevoltQuest3":
                yield return StartCoroutine(Event3());
                break;

            case "RevoltQuest4":
                yield return StartCoroutine(Event4());
                break;

            case "RevoltQuest5":
                yield return StartCoroutine(Event5());
                break;

            case "RevolotLastQuest":
                break;

        }

        onComplete?.Invoke();
        Debug.Log("End Revolt Event");
    }

    IEnumerator Event2()
    {
        // npc 생성함
        GameObject go = Instantiate(resistance, player.transform.position + new Vector3(1, 0, 0), Quaternion.identity);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueId1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // npc 나감
        Destroy(go);
    }

    IEnumerator Event3()
    {
        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueId3_0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 이동

        // 대화2
        DialogueManager.Instance.SetDialogueID(dialogueId3_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 문 닫음
        event2Door.SetActive(true);

        // 적이 엄청 생성됨
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            enemies.Add(Instantiate(cultEnemy, player.transform.position + new Vector3(i, 0, 0), Quaternion.identity));
        }


        // 적이랑 싸우면서 적을 다 죽일 때까지 기다림
        yield return new WaitUntil(() => enemies.All(e => e == null));

        // 문 엶
        event2Door.SetActive(false);
    }

    IEnumerator Event4()
    {
        // 할머니 끌고감 (이거 겹치면 쪽지루트랑 겹치면 쪽지루트 우선)

        yield return null;
    }

    IEnumerator Event5()
    {
        // 보스 등장
        GameObject go = Instantiate(resistaceBoss, player.transform.position + new Vector3(1, 0, 0), Quaternion.identity);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueId5);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 퀘스트 주기
        QuestManager.Instance.AcceptQuest(questSO.name);

        Destroy(go);
    }
}
