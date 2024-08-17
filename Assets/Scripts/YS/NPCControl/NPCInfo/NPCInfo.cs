using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;

public class NPCInfo : MonoBehaviour
{
    [Header("NPC Type")]
    public int EnemyID;
    public int side;
    public string type;

    [Header("NPC HP")]
    public float health;
    public float maxHealth;

    [Header("NPC Attack")]
    public float damage;
    public float attackRange;
    public float attackSpeed;


    [Header("NPC Aim")]
    public GameObject target;


    [Header("NPC State")]
    public bool isPatrol;
    public bool isBattle;
    public int state;


    public GameObject blood;

    private void Start()
    {
        maxHealth = 10;
        health = maxHealth;
        switch (side)
        {
            case 1:
                BattleManager.Instance.Cult.Add(gameObject);
                break;

            case 2:
                BattleManager.Instance.Resistance.Add(gameObject);
                break;
        }

        StartCoroutine(FindTargets());
    }

    private void Update()
    {


        if (BattleManager.Instance.Cult.Count > 0 && BattleManager.Instance.Resistance.Count > 1)
        {
            isBattle = true;
        }

        if(health < 0)
        {
            GameObject.Find("QuestManager").GetComponent<QuestManager>().OnEnemyKilled(EnemyID);
            Destroy(gameObject);
        }
    }

    public void FindTarget()
    {
        if (side == 1 && BattleManager.Instance.Resistance.Count > 0)
        {
            int num = 0;
            float distance;
            float minDis = 50;

            if (type == "Long")
            {
                for (int i = 0; i < BattleManager.Instance.Resistance.Count; i++)
                {
                    if (BattleManager.Instance.Resistance[i] != null)
                    {
                        distance = Vector2.Distance(transform.position, BattleManager.Instance.Resistance[i].transform.position);

                        if (distance < minDis)
                        {
                            minDis = distance;
                            num = i;
                        }
                    }
                }
            }
            else if(type == "Short")
            {
                for (int i = 0; i < BattleManager.Instance.Resistance.Count; i++)
                {
                    if (BattleManager.Instance.Resistance[i] != null )
                    {
                        if(!BattleManager.Instance.Resistance[i].CompareTag("Player"))
                        {
                            if (BattleManager.Instance.Resistance[i].GetComponent<NPCInfo>().type == ("Long"))
                            {
                                distance = Vector2.Distance(transform.position, BattleManager.Instance.Resistance[i].transform.position);

                                if (distance < minDis)
                                {
                                    minDis = distance;
                                    num = i;
                                }
                            }

                        }
                    }
                
                }
            }
            

            target = BattleManager.Instance.Resistance[num];
        }

        else if (side == 2 && BattleManager.Instance.Cult.Count > 0)
        {
            int num = 0;
            float distance;
            float minDis = 50;

            if (type == "Long")
            {
                for (int i = 0; i < BattleManager.Instance.Cult.Count; i++)
                {
                    if (BattleManager.Instance.Cult[i] != null)
                    {
                        distance = Vector2.Distance(transform.position, BattleManager.Instance.Cult[i].transform.position);

                        if (distance < minDis)
                        {
                            minDis = distance;
                            num = i;
                        }
                    }
                }

            }
            else if (type == "Short")
            {

                for (int i = 0; i < BattleManager.Instance.Cult.Count; i++)
                {
                    if (BattleManager.Instance.Cult[i] != null)
                    {
                        if (!BattleManager.Instance.Resistance[i].CompareTag("Player"))
                        {
                            if(BattleManager.Instance.Cult[i].GetComponent<NPCInfo>().type == ("Long"))
                            {
                                distance = Vector2.Distance(transform.position, BattleManager.Instance.Cult[i].transform.position);

                                if (distance < minDis)
                                {
                                    minDis = distance;
                                    num = i;
                                }

                            }
                        }
                    }
                }
            }
            target = BattleManager.Instance.Cult[num];
        }
    }

    IEnumerator FindTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            FindTarget();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health--;
            float randX = UnityEngine.Random.Range(transform.position.x - 1, transform.position.x + 1);
            float randY = UnityEngine.Random.Range(transform.position.y - 1, transform.position.y + 1);

            Instantiate(blood, new Vector3(randX, randY, 0), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
        }
    }

}
