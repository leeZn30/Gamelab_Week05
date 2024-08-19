using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteEvent : MonoBehaviour
{

    [Header("Player")]
    GameObject player;

    [Header("프리팹")]
    [SerializeField] GameObject resistanceDefault;
    [SerializeField] GameObject cultEnemy;
    [SerializeField] GameObject resistanceEnemy;

    [Header("Event1")]
    public int dialogueId1_0; // 대사 ID, 이 NPC가 말할 대사를 지정
    public int dialogueId1_1; // 대사 ID, 이 NPC가 말할 대사를 지정

    [Header("Event2")]
    public DoorInteraction event2Door;
    public int dialogueId2; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] List<Vector2> enemyPositions2;

    [Header("Event3")]
    public int dialogueId3; // 대사 ID, 이 NPC가 말할 대사를 지정

    [Header("Event4")]
    public int dialogueId4; // 대사 ID, 이 NPC가 말할 대사를 지정

    [Header("Event5")]
    public int dialogueId5; // 대사 ID, 이 NPC가 말할 대사를 지정

    [Header("Event6")]
    [SerializeField] QuestSO questSO1;
    [SerializeField] QuestSO questSO2;

    [Header("FinalBattle")]
    [SerializeField] Vector2 playerPose;
    [SerializeField] GameObject teleport;
    [SerializeReference] GameObject Girl0;
    [SerializeField] int dialogueIdF_0;
    [SerializeField] int dialogueIdF_1;
    [SerializeField] List<Vector2> enemyPositionsF = new List<Vector2>();
    [SerializeField] List<NPCInfo> enemyPrefabs = new List<NPCInfo>();

    [Header("FinalBattleAfterChoice")]
    [SerializeField] int dialogueIdFC_0;
    [SerializeField] int dialogueIdFC_1;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public IEnumerator DoEvent(Action onComplete, int eventNum)
    {
        Debug.Log("Start Note Event" + eventNum);

        switch (eventNum)
        {
            case 1:
                yield return NoteRouteManager.Instance.currentEvent = StartCoroutine(Event1());
                break;

            case 2:
                yield return NoteRouteManager.Instance.currentEvent = StartCoroutine(Event2());
                break;


            case 5:
                yield return NoteRouteManager.Instance.currentEvent = StartCoroutine(Event5());
                break;


            case 6:
                yield return NoteRouteManager.Instance.currentEvent = StartCoroutine(Event6());
                break;

            default:
                break;
        }

        onComplete?.Invoke();
        Debug.Log("End Note Event");
    }

    IEnumerator Event1()
    {
        // npc 생성함
        GameObject go = Instantiate(resistanceDefault, player.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        if (player.transform.position.x - go.transform.position.x <= 0)
        {
            go.GetComponent<SpriteRenderer>().flipX = true;
        }

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueId1_0);
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
        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueId1_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);
    }

    IEnumerator Event2()
    {
        // 문 닫음
        event2Door.CloseDoor();

        // 적이 엄청 생성됨
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < enemyPositions2.Count; i++)
        {
            enemies.Add(Instantiate(cultEnemy, enemyPositions2[i], Quaternion.identity));
            yield return new WaitForSeconds(0.2f);
        }

        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueId2);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 적이랑 싸우면서 적을 다 죽일 때까지 기다림
        yield return new WaitUntil(() => enemies.All(e => e == !e.activeSelf));

        // 문 엶
        event2Door.OpenDoor();
    }

    IEnumerator Event5()
    {
        // npc 생성함
        GameObject go = Instantiate(resistanceDefault, player.transform.position + new Vector3(1, 0, 0), Quaternion.identity);

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

    }

    IEnumerator Event6()
    {
        // Quest 실행하게 하기
        QuestManager.Instance.AcceptQuest(questSO1.questName);
        QuestManager.Instance.AcceptQuest(questSO2.questName);

        yield return null;
    }

    // 마지막 퀘스트 완료하면 전투 해야함
    public IEnumerator FinalBattle()
    {
        player.transform.position = playerPose;
        yield return new WaitForSeconds(0.5f);

        teleport.SetActive(false);
        Girl0.SetActive(true);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueIdF_0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 적이 엄청 생성됨
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < enemyPositionsF.Count; i++)
        {
            enemies.Add(Instantiate(enemyPrefabs[i], enemyPositionsF[i], Quaternion.identity).gameObject);
            yield return new WaitForSeconds(0.2f);
        }

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueIdF_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);
        yield return new WaitForSeconds(0.5f);

        // 전투
        yield return new WaitUntil(() => enemies.All(e => e == !e.activeSelf));

        // 장면 전환 (페이드 아웃?)

        // 대화함
        Debug.Log("컷씬으로 넘어가기");

        teleport.SetActive(false);
    }

    public IEnumerator FinalBattleAfterChoice()
    {
        teleport.SetActive(false);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueIdFC_0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        GameObject.Find("AllyBoss(Clone)").SetActive(false);

        // 적이 엄청 생성됨
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < enemyPositionsF.Count; i++)
        {
            enemies.Add(Instantiate(enemyPrefabs[i], enemyPositionsF[i], Quaternion.identity).gameObject);
            yield return new WaitForSeconds(0.2f);
        }

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueIdFC_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);
        yield return new WaitForSeconds(0.5f);

        // 전투
        yield return new WaitUntil(() => enemies.All(e => e == !e.activeSelf));

        // 장면 전환 (페이드 아웃?)

        // 대화함
        Debug.Log("컷씬으로 넘어가기");
        CutsceneManager.Instance.PlayCutsceneByName("02_NoteEnding");

        teleport.SetActive(false);
    }

}
