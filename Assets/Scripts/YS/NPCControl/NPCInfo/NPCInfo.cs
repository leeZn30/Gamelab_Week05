using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;

public struct NPCInfoStatus
{
    public float health;
    public Vector2 position;
    public Quaternion rotation;
    public bool isBattle;

    public NPCInfoStatus(float saveHealth, bool saveBattle, Vector2 savePosition, Quaternion saveRotation)
    {
        health = saveHealth;
        position = savePosition;
        rotation = saveRotation;
        isBattle = saveBattle;
    }
}

public class NPCInfo : MonoBehaviour, IListener
{
    [Header("NPC Type")] 
    public bool questNPC;
    public int EnemyID;
    private int hitTime;
    public int side;
    public string type;
    private Vector2 startPos;

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


    [Header("Damaged")] public GameObject damaged;


    public float distaance;
    public GameObject blood;
    public int saveIndex = -1;
    public bool isDeath = false;

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Awake()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
    }

    private void Start()
    {
        startPos = transform.position;
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
            if (distaance > resetDistance) //&& (BattleManager.Instance.Cult.Count > 0 && BattleManager.Instance.Resistance.Count > 1))
            {
                if (questNPC)
                {
                    GetComponent<NPCMovement>().targetPosition = startPos;
                }
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
        /*
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
        */
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
                        distance = Vector2.Distance(transform.position,
                            BattleManager.Instance.Resistance[i].transform.position);

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
                distance = Vector2.Distance(transform.position,
                    BattleManager.Instance.Resistance[i].transform.position);

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
                        distance = Vector2.Distance(transform.position,
                            BattleManager.Instance.Cult[i].transform.position);

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
            //Destroy(gameObject);
            if (questNPC)
            {
                QuestNPCInteraction interaction = GetComponent<QuestNPCInteraction>();
                if (interaction != null)
                    interaction.isDeath = true;
            }
            else
            {
                QuestNPCInteraction interaction = GetComponent<QuestNPCInteraction>();
                if (interaction != null)
                    interaction.isDeath = true;
            }
            SaveManager.Instance.tempNPCDestroy.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
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

                if (hitTime >= 5)
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

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
        if (!isDeath)
        {
            switch (EventType)
            {
                case Event_Type.eSave:
                    NPCInfoStatus npcInfoStatus = new NPCInfoStatus(health, isBattle, gameObject.transform.position, gameObject.transform.rotation);
                    SaveManager.Instance.saveNpcInfoStatus.Add(npcInfoStatus);
                    saveIndex = SaveManager.Instance.saveNpcInfoStatus.Count - 1;
                    break;
                case Event_Type.eLoad:
                    if (saveIndex == -1)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        health = SaveManager.Instance.saveNpcInfoStatus[saveIndex].health;
                        transform.position = SaveManager.Instance.saveNpcInfoStatus[saveIndex].position;
                        transform.rotation = SaveManager.Instance.saveNpcInfoStatus[saveIndex].rotation;
                        if (SaveManager.Instance.saveNpcInfoStatus[saveIndex].isBattle)
                        {
                            isBattle = true;
                            weapon.SetActive(false);
                        }
                    }
                    break;
            }
        }
    }
}