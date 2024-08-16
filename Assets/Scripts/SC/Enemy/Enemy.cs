using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int EnemyID;
    public void Die()
    {
        GameObject.Find("QuestManager").GetComponent<QuestManager>().OnEnemyKilled(EnemyID);
        Destroy(gameObject);
    }
}

