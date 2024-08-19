using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevoltRouteManager : Singleton<RevoltRouteManager>, IListener
{
    [Header("현재 이벤트")]
    public Coroutine currentEvent;

    [Header("반란 이벤트")]
    public List<QuestNPCInteraction> QuestNPCs = new List<QuestNPCInteraction>();
    public int currentNPCOrder = 0;
    [SerializeField] RevoltEvent revoltEvent;
    QuestSO currentQuestSO;

    [Header("최종 루트 진입 직전")]
    public bool isLast;

    void Awake()
    {
        // 씬 이동해도 삭제되면 안됨
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }

        // 이벤트 등록
        EventManager.Instance.AddListener(Event_Type.eRevoltQuestDone, this);
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eRevoltQuestDone:
                currentQuestSO = (QuestSO)Param;
                StartCoroutine(EventCheck());

                // 다음 순서 NPC 활성화
                currentNPCOrder++;
                QuestNPCInteraction npc = QuestNPCs.Find(e => e.order == currentNPCOrder);
                if (npc != null)
                    npc.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator EventCheck()
    {
        bool isComplete = false;
        StartCoroutine(revoltEvent.DoEvent(() => isComplete = true, currentQuestSO));
        yield return new WaitUntil(() => isComplete);
    }

    public void CallFinalEvent()
    {
        StartCoroutine(revoltEvent.FinalBattle());
    }

    public void CallFinalEventAfterChoice()
    {
        StartCoroutine(revoltEvent.FinalBattleAfterChoice());
    }
}
