using UnityEngine;
using System.Collections;


public class BookshelfInteraction : MonoBehaviour
{
    private float moveDistance = 1.0f; // 책장이 이동할 거리
    private float moveSpeed = 2.0f; // 책장이 이동하는 속도
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    private bool isMoving = false; // 책장이 이동 중인지 확인
    private Vector3 initialPosition; // 책장의 초기 위치
    private Vector3 targetPosition; // 책장이 이동할 목표 위치

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
            Debug.Log("Player entered the range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player exited the range.");
        }
    }

}
