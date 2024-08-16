using System.Collections;
using UnityEngine;

public class NPCShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform rotation; // rotation Ÿ���� GameObject���� Transform���� ����

    private bool shooting;

    public float shootDelay = 1f;
    private float shootTime;

    private void Start()
    {
        rotation = transform.parent.parent; // parent�κ��� Transform�� ������
        StartCoroutine(ShootDelay());
    }

    void Update()
    {
        // rotation�� forward �������� Ray�� ���ϴ�.
        Vector2 direction = rotation.right; // 2D������ forward ��� right�� ���
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction);

        // ����׿� ���� ǥ��
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
