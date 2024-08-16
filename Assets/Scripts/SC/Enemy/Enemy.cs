using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDeath(Enemy enemy);
    public static event EnemyDeath OnEnemyDeath;

    public void Die()
    {
        // ���� ���� �� ����Ǵ� �ڵ�
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }
}

