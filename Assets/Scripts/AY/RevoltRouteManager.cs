using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevoltRouteManager : Singleton<RevoltRouteManager>, IListener
{
    [Header("반란 이벤트")]
    [SerializeField] RevoltEvent revoltEvent;
    int sumQuest;

    [Header("최종 루트 진입 직전")]
    [SerializeField] bool isEveLast;

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
                // currentQuest = (QuestSO)Param;
                sumQuest = (int)Param;
                StartCoroutine(EventCheck());
                break;
        }
    }

    IEnumerator EventCheck()
    {
        bool isComplete = false;
        StartCoroutine(revoltEvent.DoEvent(() => isComplete = true, sumQuest));
        yield return new WaitUntil(() => isComplete);
    }
}
