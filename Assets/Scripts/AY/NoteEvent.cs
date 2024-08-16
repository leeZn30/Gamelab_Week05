using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NoteEvent : MonoBehaviour
{
    public int currentEvent;

    [Header("Event1")]
    public int dialogueId; // 대사 ID, 이 NPC가 말할 대사를 지정
    [SerializeField] GameObject person;

    [Header("Event5")]
    [SerializeField] QuestSO questSO;

    public IEnumerator DoEvent(System.Action onComplete, int eventNum)
    {
        Debug.Log("Start Note Event");

        switch (eventNum)
        {
            case 0:
                break;

            case 1:
                yield return StartCoroutine(Event1());
                break;

            case 2:
                yield return StartCoroutine(Event2());
                break;

            case 3:
                yield return StartCoroutine(Event3());
                break;

            case 4:
                yield return StartCoroutine(Event4());
                break;

            case 5:
                yield return StartCoroutine(Event5());
                break;

            case 6:
                yield return StartCoroutine(Event6());
                break;

        }


        Debug.Log("End Note Event");
        onComplete?.Invoke();
    }

    IEnumerator Event1()
    {
        // npc 생성함
        GameObject go = Instantiate(person, Vector3.zero, Quaternion.identity);

        // 대화함
        // Dialogue dialogue = DatabaseManager.instance.GetDialogueById(dialogueId);
        // DialogueManager.Instance.StartDialogue(dialogue);

        // while (true)
        // {
        //     if (Input.GetKeyDown(KeyCode.E))
        //         // 다음 대사로 넘기기
        //         DialogueManager.Instance.DisplayNextSentence();
        // }

        yield return new WaitForSeconds(3f);

        // npc 나감
        Destroy(go);
    }

    IEnumerator Event2()
    {
        // 적이 엄청 생성됨

        // 대화


        // 적이랑 싸우면서 적을 다 죽일 때까지 기다림


        // 다시 진행함

        yield return null;
    }


    IEnumerator Event3()
    {
        // 입단 테스트가 완료되지 않았다면

        // 대화

        yield return null;
    }

    IEnumerator Event4()
    {
        // 가는 길에..?

        // 대화

        yield return null;
    }

    IEnumerator Event5()
    {
        // npc 생성함
        GameObject go = Instantiate(person, Vector3.zero, Quaternion.identity);

        // 대화함
        // Dialogue dialogue = DatabaseManager.instance.GetDialogueById(dialogueId);
        // DialogueManager.Instance.StartDialogue(dialogue);

        // while (true)
        // {
        //     if (Input.GetKeyDown(KeyCode.E))
        //         // 다음 대사로 넘기기
        //         DialogueManager.Instance.DisplayNextSentence();
        // }

        yield return new WaitForSeconds(3f);

        // npc 나감
        Destroy(go);
    }

    IEnumerator Event6()
    {
        // Quest 완료
        // 마지막 퀘슽 완료하면 전투 해야함 => DoEvent로 QuestMaanger에서 호출해야 함
        QuestManager.Instance.AcceptQuest(questSO);

        // npc 생성함
        GameObject go = Instantiate(person, Vector3.zero, Quaternion.identity);

        yield return new WaitForSeconds(3f);

        // npc 나감
        Destroy(go);
    }


}
