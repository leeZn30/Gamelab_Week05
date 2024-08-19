using System.Collections;
using UnityEngine;

public class NPCShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform rotation; // rotation 타입을 GameObject에서 Transform으로 변경
    public Transform sPoint;

    private float shootTime;

    public LayerMask layermask;

    private void OnEnable()
    {
        rotation = transform.parent.parent; // parent로부터 Transform을 가져옴
        StartCoroutine(ShootDelay());
    }

    void Update()
    {
        if (transform.parent.parent.parent.GetComponent<NPCInfo>().isBattle)
        {
            // rotation의 forward 방향으로 Ray를 쏩니다.
            Vector2 direction = rotation.right; // 2D에서는 forward 대신 right를 사용
            RaycastHit2D hit = Physics2D.Raycast(sPoint.transform.position, direction, Mathf.Infinity, layermask);

            // 디버그용 레이 표시
            Debug.DrawRay(transform.position, direction * 10f, Color.red);


            if (hit.collider != null)
            {
                if (Vector2.Distance(sPoint.transform.position, hit.point) < transform.parent.parent.parent.GetComponent<NPCInfo>().attackRange)
                {
                    if(transform.parent.parent.parent.GetComponent<NPCInfo>().side == 1)
                    {
                        // 사격 처리
                        if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Resistance"))
                        {
                            if (shootTime > transform.parent.parent.parent.GetComponent<NPCInfo>().attackSpeed)
                            {
                                shootTime = 0;
                                Instantiate(bullet, transform.position, rotation.rotation);
                                Debug.Log("Shooting bullet");
                            }
                        }

                    }
                    else if(transform.parent.parent.parent.GetComponent<NPCInfo>().side == 2)
                    {
                        // 사격 처리
                        if (hit.collider.CompareTag("Cult"))
                        {
                            if (shootTime > transform.parent.parent.parent.GetComponent<NPCInfo>().attackSpeed)
                            {
                                shootTime = 0;
                                Instantiate(bullet, transform.position, rotation.rotation);
                                Debug.Log("Shooting bullet");
                            }
                        }

                    }

                }
            }
        }

    }

    IEnumerator ShootDelay()
    {
        while (true)
        {
            yield return null;
            shootTime += Time.deltaTime;
        }
    }
}
