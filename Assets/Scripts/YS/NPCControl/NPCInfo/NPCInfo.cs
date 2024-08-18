using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;

public class NPCInfo : MonoBehaviour
{
    [Header("NPC Type")]
    public bool questNPC;

    public int EnemyID;
    private int hitTime;
    public int side;
    public string type;

    [Header("NPC HP")]
    public float health;
    public float maxHealth;

    [Header("NPC Attack")]
    public GameObject weapon;
    public float damage;
    public float attackRange;
    public float attackSpeed;

    [Header("NPC Aim")]
    public GameObject target;

    [Header("NPC State")]
    public bool isPatrol;
    public bool isBattle;
    public int state;

    [Header("Finding Target")]
    int num = 0;
    float distance;
    float minDis = 50;


    [Header("Check Battle")]
    public float radius = 5f;
    public LayerMask layerMask;
    public float resetDistance;


    [Header("Damaged")]
    public GameObject damaged;


    public float distaance;
    public GameObject blood;

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        damaged.SetActive(false);

        if (weapon != null)
        {
            weapon.SetActive(false);
        }

        hitTime = 0;
        maxHealth = 10;
        health = maxHealth;
        SetSide();

        StartCoroutine(FindTargets());
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {
        BattleCheck();

        distaance = Vector2.Distance(GameObject.FindWithTag("Player").transform.position, transform.position);
        
        if (isBattle)
        {
            if (distaance > resetDistance && (BattleManager.Instance.Cult.Count > 0 && BattleManager.Instance.Resistance.Count > 1))
            {
                isBattle = false;
                weapon.SetActive(false);
            }
        }
        

        DeathCheck();

    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void SetSide()
    {
        switch (side)
        {
            case 1:
                BattleManager.Instance.Cult.Add(gameObject);
                break;

            case 2:
                BattleManager.Instance.Resistance.Add(gameObject);
                break;
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void BattleCheck()
    {

        if (BattleManager.Instance.Cult.Count > 0 && BattleManager.Instance.Resistance.Count > 1)
        {
            if (weapon != null && !questNPC)
            {
                isBattle = true;
                weapon.SetActive(true);
            }

        }

        if (!questNPC)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].CompareTag("Player"))
                {
                    if (DataManager.Instance.playerState == "Battle")
                    {
                        if (weapon != null && !questNPC)
                        {
                            Debug.Log("PlayerIN");
                            StartCoroutine(DamagedDelay());
                        }

                    }
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void FindTarget()
    {
        if (side == 1 && BattleManager.Instance.Resistance.Count > 0)
        {

            if (type == "Long")
            {
                LongCult();
            }
            else if (type == "Short")
            {
                ShortCult();
            }

            target = BattleManager.Instance.Resistance[num];
        }

        else if (side == 2 && BattleManager.Instance.Cult.Count > 0)
        {
            if (type == "Long")
            {
                LongResistance();
            }
            else if (type == "Short")
            {
                ShortResistance();
            }
            target = BattleManager.Instance.Cult[num];
        }
    }
    void ShortCult()
    {

        for (int i = 0; i < BattleManager.Instance.Resistance.Count; i++)
        {
            if (BattleManager.Instance.Resistance[i] != null)
            {
                if (!BattleManager.Instance.Resistance[i].CompareTag("Player"))
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
    void LongCult()
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
    void ShortResistance()
    {

        for (int i = 0; i < BattleManager.Instance.Cult.Count; i++)
        {
            if (BattleManager.Instance.Cult[i] != null)
            {
                if (!BattleManager.Instance.Resistance[i].CompareTag("Player"))
                {
                    if (BattleManager.Instance.Cult[i].GetComponent<NPCInfo>().type == ("Long"))
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
    void LongResistance()
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

    IEnumerator FindTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            FindTarget();
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void DeathCheck()
    {
        if (health < 0)
        {
            GameObject.Find("QuestManager").GetComponent<QuestManager>().OnEnemyKilled(EnemyID);
            Destroy(gameObject);
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health--;
            float randX = UnityEngine.Random.Range(transform.position.x - 1, transform.position.x + 1);
            float randY = UnityEngine.Random.Range(transform.position.y - 1, transform.position.y + 1);

            Instantiate(blood, new Vector3(randX, randY, 0), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            DataManager.Instance.playerState = "Battle";


            if (weapon != null && !questNPC)
            {
                StartCoroutine(DamagedDelay());
            }


            if (weapon != null && questNPC)
            {
                hitTime++;

                if(hitTime >= 5)
                {
                    StartCoroutine(DamagedDelay());
                }
            }

        }
    }

    IEnumerator DamagedDelay()
    {
        if (damaged != null)
        {
            damaged.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            isBattle = true;
            weapon.SetActive(true);
            damaged.SetActive(false);
        }
    }

}
