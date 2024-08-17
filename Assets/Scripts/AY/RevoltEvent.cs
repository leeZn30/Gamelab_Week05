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

    [Header("Event1")]
    [SerializeField] int dialogueId1;

    [Header("Event2")]
    [SerializeField] int dialogueId3_0;
    [SerializeField] int dialogueId3_1;
    [SerializeField] GameObject event2Door;

    [Header("Event67")]
    [SerializeField] int dialogueId5;
    [SerializeField] QuestSO questSO;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public IEnumerator DoEvent(Action onComplete, int questNum)
    {
        Debug.Log("Start Revolt Event");

        switch (questNum)
        {
            case 1:
                // yield return StartCoroutine(Event1());
                break;

            case 2:
                yield return StartCoroutine(Event2());
                break;

            case 4:
                // yield return StartCoroutine(Event34());
                break;

            case 7:
                yield return StartCoroutine(Event67());
                break;

            case 8: // last
                break;
        }

        onComplete?.Invoke();
        Debug.Log("End Revolt Event");
    }

    IEnumerator Event1()
    {
        // npc 생성함
        GameObject go = Instantiate(resistance, player.transform.position + new Vector3(1, 0, 0), Quaternion.identity);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueId1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // npc 나감
        Destroy(go);
    }

    IEnumerator Event2()
    {
        // 쿵 소리
        yield return new WaitForSeconds(0.5f);

        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueId3_0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 문 닫음 (뭐 부터 깰지 몰라서 없애두기)
        // event2Door.SetActive(true);

        // 말한 반란군 잠시 안보이게 하기 (미완)

        // 적이 엄청 생성됨
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            enemies.Add(Instantiate(cultEnemy, player.transform.position + new Vector3(i, 0, 0), Quaternion.identity));
        }
        // 적이랑 싸우면서 적을 다 죽일 때까지 기다림
        yield return new WaitUntil(() => enemies.All(e => e == null));

        // 대화2
        DialogueManager.Instance.SetDialogueID(dialogueId3_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 문 엶 (뭐 부터 깰지 몰라서 없애두기)
        event2Door.SetActive(false);
    }

    IEnumerator Event34()
    {
        // 할머니 끌고감 (이거 겹치면 쪽지루트랑 겹치면 쪽지루트 우선)

        yield return null;
    }

    IEnumerator Event67()
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
