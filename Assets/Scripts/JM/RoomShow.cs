using UnityEngine;

public class RoomShow : MonoBehaviour, IListener
{
    private Transform originalParent; // 원래 부모 오브젝트의 Transform\
    private bool isSeperated = false;

    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    void Start()
    {
        // 스크립트가 처음 시작될 때 원래 부모를 저장해 둡니다.
        originalParent = transform.parent;
        //transform.SetParent(originalParent);
    }

    void Update()
    {
        // 부모 오브젝트가 활성화되었고, 자신이 이미 분리되지 않았을 경우
        if (originalParent != null && originalParent.gameObject.activeSelf && transform.parent == originalParent)
        {
            // 부모로부터 분리시킵니다.
            transform.SetParent(null);
        }
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eSave:
                if (isSeperated)
                {
                    SaveManager.Instance.isSeperated = true;
                }
                else
                {
                    SaveManager.Instance.isSeperated = false;
                }
                break;
            case Event_Type.eLoad:
                if (SaveManager.Instance.isSeperated)
                {
                    transform.SetParent(null);
                }
                else
                {
                    transform.SetParent(originalParent);
                }
                break;
        }
    }
}
