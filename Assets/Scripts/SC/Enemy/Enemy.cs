using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDeath(Enemy enemy);
    public static event EnemyDeath OnEnemyDeath;

    public void Die()
    {
        // 적이 죽을 때 실행되는 코드
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }
}

