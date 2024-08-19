using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevoltRouteManager : Singleton<RevoltRouteManager>, IListener
{
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
        EventManager.Instance.AddListener(Event_Type.eRevoltLastQuestDone, this);
    }

    void Start()
    {
        QuestManager.Instance.AcceptQuest("RevoltLastQuest");
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eRevoltQuestDone:
                // sumQuest = (int)Param;
                currentQuestSO = (QuestSO)Param;
                StartCoroutine(EventCheck());

                // 다음 순서 NPC 활성화
                currentNPCOrder++;
                if (QuestNPCs.Find(e => e.order == currentNPCOrder) != null)
                    QuestNPCs.Find(e => e.order == currentNPCOrder).gameObject.SetActive(true);
                break;

            case Event_Type.eRevoltLastQuestDone:
                isLast = true;
                CommonRouteManager.Instance.LastChoiceEventCheck();
                break;
        }
    }

    IEnumerator EventCheck()
    {
        bool isComplete = false;
        StartCoroutine(revoltEvent.DoEvent(() => isComplete = true, currentQuestSO));
        yield return new WaitUntil(() => isComplete);
    }

    public IEnumerator CallRevoltFinalEvent()
    {
        bool isComplete = false;
        StartCoroutine(revoltEvent.DoEvent(() => isComplete = true, "RevoltLastQuest"));
        yield return new WaitUntil(() => isComplete);
    }
}
