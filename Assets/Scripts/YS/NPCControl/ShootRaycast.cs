using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRaycast : MonoBehaviour
{
    public float radius = 5f; // Raycast의 길이
    public int numRays = 20; // 발사할 Raycast의 개수
    public float coneAngle = 60f; // 삼각형의 각도
    public LayerMask layerMask; // 체크할 레이어

    void Update()
    {
        radius = 5;
        coneAngle = 60;

        PerformRadialRaycast();
    }

    void PerformRadialRaycast()
    {
        if (transform.parent.parent.parent.parent.GetComponent<NPCInfo>().isPatrol)
        {
            float halfConeAngle = coneAngle / 2f;
            float angleStep = coneAngle / (numRays - 1);

            for (int i = 0; i < numRays; i++)
            {
                float angle = -halfConeAngle + i * angleStep;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * transform.right;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, radius, layerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        DataManager.Instance.isDectected = true;
                    }
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    // 여기서 히트된 오브젝트에 대한 로직을 처리할 수 있습니다.
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position + direction * radius, Color.green);
                }

            }

        }
    }
}

