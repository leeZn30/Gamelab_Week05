using UnityEngine;

public class StatueCollision : MonoBehaviour, IListener
{
    public GameObject childObjectToActivate; // 활성화할 자식 오브젝트
    public GameObject childObjectToDisactivate; // 활성화할 자식 오브젝트

    private int collisionCount = 0; // 충돌 횟수
    private int maxCollisions = 4; // 최대 충돌 횟수

    public bool isBreak = false;
    public int saveIndex = -1;

    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 Bullet 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collisionCount++; // 충돌 횟수 증가// 충돌 횟수가 4번 이상이면

            if (collisionCount >= maxCollisions)
            {
                if (childObjectToActivate != null)
                {
                    isBreak = true;
                    childObjectToDisactivate.SetActive(false); // 자식 오브젝트 활성화
                    childObjectToActivate.SetActive(true); // 자식 오브젝트 활성화
                }

            }
        }
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
            switch (EventType)
            {
                case Event_Type.eSave:
                    if (isBreak)
                    {
                        SaveManager.Instance.saveStatueCollision.Add(true);
                        saveIndex = SaveManager.Instance.saveStatueCollision.Count - 1;
                    }
                    else
                    {
                        SaveManager.Instance.saveStatueCollision.Add(false);
                        saveIndex = SaveManager.Instance.saveStatueCollision.Count - 1;
                    }
                    break;
                case Event_Type.eLoad:
                    if (SaveManager.Instance.saveStatueCollision[saveIndex])
                    {
                        childObjectToDisactivate.SetActive(false); // 자식 오브젝트 활성화
                        childObjectToActivate.SetActive(true); // 자식 오브젝트 활성화
                    }
                    else
                    {
                        childObjectToDisactivate.SetActive(true); // 자식 오브젝트 활성화
                        childObjectToActivate.SetActive(false); // 자식 오브젝트 활성화
                    }
                    break;
            }
    }
}
