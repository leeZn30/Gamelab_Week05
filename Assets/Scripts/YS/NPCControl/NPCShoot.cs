using System.Collections;
using UnityEngine;

public class NPCShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform rotation; // rotation 타입을 GameObject에서 Transform으로 변경

    private bool shooting;

    public float shootDelay = 1f;
    private float shootTime;

    private void Start()
    {
        rotation = transform.parent.parent; // parent로부터 Transform을 가져옴
        StartCoroutine(ShootDelay());
    }

    void Update()
    {
        // rotation의 forward 방향으로 Ray를 쏩니다.
        Vector2 direction = rotation.right; // 2D에서는 forward 대신 right를 사용
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction);

        // 디버그용 레이 표시
        Debug.DrawRay(transform.position, direction * 10f, Color.red);

        for(int i = 0; i< hit.Length; i++)
        {
            if (hit[i].collider != null)
            {
                if (hit[i].collider.CompareTag("Wall"))
                {
                    shooting = false;
                    break;
                }
                if (hit[i].collider.CompareTag("Player"))
                {
                    shooting = true;
                    break;
                }
            }

        }

        if (shooting)
        {
            if (shootTime > shootDelay)
            {
                shootTime = 0;
                Instantiate(bullet, transform.position, rotation.rotation);
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
