using UnityEngine;
using System.Collections;
public struct BookshelfInteractionStatus
{
    public bool isOpend;
    public BookshelfInteractionStatus(bool currOpend)
    {
        isOpend = currOpend;
    }
}

public class BookshelfInteraction : MonoBehaviour, IListener
{
    private float moveDistance = 1.0f; // 책장이 이동할 거리
    private float moveSpeed = 2.0f; // 책장이 이동하는 속도
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    private bool isMoving = false; // 책장이 이동 중인지 확인
    public bool isOpened = false;
    public int saveIndex = -1;
    private Vector3 initialPosition; // 책장의 초기 위치
    private Vector3 targetPosition; // 책장이 이동할 목표 위치

    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.left * moveDistance; // 왼쪽으로 이동할 목표 위치 설정
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            StartCoroutine(MoveBookshelf());
            isOpened = true;
        }
    }

    private IEnumerator MoveBookshelf()
    {
        isMoving = true;

        // 책장을 목표 위치로 이동시킵니다.
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // 정확한 위치 보정
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        switch (EventType)
        {
            case Event_Type.eSave:
                if (isOpened)
                {
                    SaveManager.Instance.saveBookShelfInteraction.Add(true);
                }
                else
                {
                    SaveManager.Instance.saveBookShelfInteraction.Add(false);
                }
                saveIndex = SaveManager.Instance.saveBookShelfInteraction.Count - 1;
                break;
            case Event_Type.eLoad:
                if (saveIndex != -1)
                {
                    if (SaveManager.Instance.savedDoorStatus[saveIndex].isOpend)
                    {
                        transform.position = targetPosition;
                    }
                    else
                    {
                        transform.position = initialPosition;
                    }
                }
                break;
        }
    }
}
