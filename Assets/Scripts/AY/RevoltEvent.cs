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
    [SerializeField] GameObject resistanceDefault;
    [SerializeField] GameObject cultEnemy;
    [SerializeField] GameObject resistaceBoss;

    [Header("Event1")]
    [SerializeField] int dialogueId1;

    [Header("Event2")]
    [SerializeField] int dialogueId3_0;
    [SerializeField] int dialogueId3_1;
    [SerializeField] DoorInteraction event2Door;
    [SerializeField] List<Vector2> enemyPositions;

    [Header("Event67")]
    [SerializeField] int dialogueId5;
    [SerializeField] QuestSO questSO;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public IEnumerator DoEvent(Action onComplete, QuestSO questSO)
    {
        Debug.Log("Start Revolt Event");

        switch (questSO.questName)
        {
            case "RevoltQuest2":
                yield return StartCoroutine(Event2());
                break;

            case "RevoltQuest67":
                yield return StartCoroutine(EventMeetBoss());
                break;

            case "RevoltLastQuest": // last
                break;
        }

        onComplete?.Invoke();
        Debug.Log("End Revolt Event");
    }

    IEnumerator Event1()
    {
        // npc 생성함
        GameObject go = Instantiate(resistanceDefault, player.transform.position + new Vector3(1, 0, 0), Quaternion.identity);

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

        // 문 닫음
        event2Door.CloseDoor();

        // 적이 엄청 생성됨
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < enemyPositions.Count; i++)
        {
            enemies.Add(Instantiate(cultEnemy, enemyPositions[i], Quaternion.identity));
            yield return new WaitForSeconds(0.3f);
        }
        // 적이랑 싸우면서 적을 다 죽일 때까지 기다림
        yield return new WaitUntil(() => enemies.All(e => e == null));

        // 대화2
        DialogueManager.Instance.SetDialogueID(dialogueId3_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 문 엶
        event2Door.OpenDoor();
    }

    IEnumerator EventMeetBoss()
    {
        // 보스 등장
        GameObject go = Instantiate(resistaceBoss, player.transform.position + new Vector3(-1, 0, 0), Quaternion.identity);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueId5);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // npc 나감
        SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            // 투명도 조절
            float alpha = Mathf.Clamp01(1 - (elapsedTime / 1f));
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null; // 다음 프레임까지 대기
        }
        // 페이드 아웃이 완료된 후 오브젝트를 비활성화 또는 삭제
        Destroy(go); // 오브젝트를 삭제하고 싶다면 이 줄을 사용하세요.

        // 퀘스트 주기
        QuestManager.Instance.AcceptQuest(questSO.name);
    }

    IEnumerator FinalBattle()
    {
        yield return null;
    }
}
