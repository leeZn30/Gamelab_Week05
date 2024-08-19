using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class RevoltEvent : MonoBehaviour
{
    [Header("Player")]
    GameObject player;

    [Header("프리팹")]
    [SerializeField] GameObject resistanceDefault;
    [SerializeField] GameObject cultEnemy;
    [SerializeField] GameObject resistaceBoss;
    [SerializeField] GameObject cultBoss;

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

    [Header("FinalBattle")]
    [SerializeField] Vector2 playerPose;
    [SerializeField] GameObject teleport;
    [SerializeReference] GameObject Girl;
    [SerializeField] int dialogueIdF_0;
    [SerializeField] int dialogueIdF_1;
    [SerializeField] int dialogueIdF_2;
    [SerializeField] int dialogueIdF_3;
    [SerializeField] List<Vector2> allyPositionsF = new List<Vector2>();
    [SerializeField] List<NPCInfo> allyPrefabsF = new List<NPCInfo>();
    [SerializeField] List<Vector2> enemyPositionsF = new List<Vector2>();

    [Header("FinalBattleAfterChoice")]
    [SerializeField] int dialogueIdFC_0;
    [SerializeField] int dialogueIdFC_1;
    [SerializeField] int dialogueIdFC_2;


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
                yield return RevoltRouteManager.Instance.currentEvent = StartCoroutine(Event2());
                break;

            case "RevoltQuest67":
                yield return RevoltRouteManager.Instance.currentEvent = StartCoroutine(EventMeetBoss());
                break;
        }

        onComplete?.Invoke();
        Debug.Log("End Revolt Event");
    }

    public IEnumerator DoEvent(Action onComplete, string name)
    {
        Debug.Log("Start Revolt Event");

        switch (name)
        {
            case "RevoltLastQuest": // last
                yield return RevoltRouteManager.Instance.currentEvent = StartCoroutine(FinalBattle());
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
        yield return new WaitUntil(() => enemies.All(e => e == !e.activeSelf));

        // 대화2
        DialogueManager.Instance.SetDialogueID(dialogueId3_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 문 엶
        event2Door.OpenDoor();
    }

    IEnumerator EventMeetBoss()
    {
        // 보스 등장
        GameObject go = Instantiate(resistaceBoss, player.transform.position + new Vector3(-1, 0.5f, 0), Quaternion.identity);

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

    public IEnumerator FinalBattle()
    {
        player.transform.position = playerPose;
        yield return new WaitForSeconds(0.5f);

        teleport.SetActive(false);
        Girl.SetActive(true);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueIdF_0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 보스 등장
        Instantiate(allyPrefabsF[0], allyPositionsF[0], quaternion.identity);

        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueIdF_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 교주랑 광신도 등장
        List<GameObject> enemies = new List<GameObject>
        {
            Instantiate(cultBoss, enemyPositionsF[0], Quaternion.identity).gameObject
        };
        yield return new WaitForSeconds(0.2f);
        for (int i = 1; i < enemyPositionsF.Count; i++)
        {
            enemies.Add(Instantiate(cultEnemy, enemyPositionsF[i], Quaternion.identity).gameObject);
        }

        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueIdF_2);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 반란군 도와주러 옴
        for (int i = 1; i < allyPositionsF.Count; i++)
        {
            Instantiate(allyPrefabsF[i], allyPositionsF[i], Quaternion.identity);
            // yield return new WaitForSeconds(0.1f);
        }

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueIdF_3);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);
        yield return new WaitForSeconds(0.5f);

        Girl.SetActive(false);

        // 전투
        yield return new WaitUntil(() => enemies.All(e => e == !e.activeSelf));

        // 장면 전환 (페이드 아웃?)

        // 대화함
        Debug.Log("컷씬으로 넘어가기");
        yield return TurnEnd.Instance.StartCoroutine(TurnEnd.Instance.FadeInAndOut());

        SceneManager.LoadScene("03_QuestEnding");

        teleport.SetActive(false);
    }

    public IEnumerator FinalBattleAfterChoice()
    {
        teleport.SetActive(false);

        // 대화함
        DialogueManager.Instance.SetDialogueID(dialogueIdFC_0);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 교주랑 광신도 등장
        List<GameObject> enemies = new List<GameObject>
        {
            Instantiate(cultBoss, enemyPositionsF[0], Quaternion.identity).gameObject
        };
        yield return new WaitForSeconds(0.2f);
        for (int i = 1; i < enemyPositionsF.Count; i++)
        {
            enemies.Add(Instantiate(cultEnemy, enemyPositionsF[i], Quaternion.identity).gameObject);
        }

        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueIdFC_1);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        // 반란군 도와주러 옴
        for (int i = 1; i < allyPositionsF.Count; i++)
        {
            Instantiate(allyPrefabsF[i], allyPositionsF[i], Quaternion.identity);
            // yield return new WaitForSeconds(0.1f);
        }

        // 대화
        DialogueManager.Instance.SetDialogueID(dialogueIdFC_2);
        yield return new WaitUntil(() => !DialogueManager.Instance.isDialogueActive);

        Girl.SetActive(false);

        // 전투
        yield return new WaitUntil(() => enemies.All(e => e == !e.activeSelf));

        // 장면 전환 (페이드 아웃?)

        // 대화함
        Debug.Log("컷씬으로 넘어가기");
        yield return TurnEnd.Instance.StartCoroutine(TurnEnd.Instance.FadeInAndOut());
        SceneManager.LoadScene("03_QuestEnding");

        teleport.SetActive(false);
    }


}
