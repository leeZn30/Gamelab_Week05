using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : Singleton<EventManager>
{
    Dictionary<Event_Type, List<IListener>> Listeners = new Dictionary<Event_Type, List<IListener>>();

    void Awake()
    {
        // 씬 이동해도 삭제되면 안됨
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        // else
        // {
        //     DestroyImmediate(gameObject);
        // }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies();
    }



    // 리스너 등록
    public void AddListener(Event_Type eventType, IListener listener)
    {
        List<IListener> ListenList = null;

        if (Listeners.TryGetValue(eventType, out ListenList))
        {
            ListenList.Add(listener);
            return;
        }

        ListenList = new List<IListener>();
        ListenList.Add(listener);
        Listeners.Add(eventType, ListenList);
    }


    // 이벤트 발생시 해당 이벤트를 받는 리스너에게 알려줌
    public void PostNotification(Event_Type eventType, Component sender, object param = null)
    {
        List<IListener> ListenList = null;

        // 해당 이벤트를 받는 리스너가 없으면 리턴
        if (!Listeners.TryGetValue(eventType, out ListenList))
            return;

        // 해당 이벤트를 받는 리스너 모두 실행
        for (int i = 0; i < ListenList.Count; i++)
            ListenList?[i].OnEvent(eventType, sender, param);
    }

    // 더이상 사용하지 않는 이벤트 지우기
    public void RemoveEvent(Event_Type event_Type) => Listeners.Remove(event_Type);

    // 씬이 바뀌어서 이미 파괴된 오브젝트를 참조하려고 하면 안됨
    // 씬 로드할때 호출
    public void RemoveRedundancies()
    {
        Dictionary<Event_Type, List<IListener>> newListeners = new Dictionary<Event_Type, List<IListener>>();

        foreach (KeyValuePair<Event_Type, List<IListener>> Item in Listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            if (Item.Value.Count > 0)
                newListeners.Add(Item.Key, Item.Value);
        }

        Listeners = newListeners;
    }

}
