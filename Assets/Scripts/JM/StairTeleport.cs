using UnityEngine;

publicclassStairTeleport : MonoBehaviour
{
    public Transform targetPosition; // 플레이어가 이동할 목표 위치private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 위치를 목표 위치로 이동
            other.transform.position = targetPosition.position;
        }
    }
}
