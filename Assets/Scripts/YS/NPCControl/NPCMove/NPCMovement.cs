using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private Transform playerTransform;
    private GameObject[] walls;
    Vector3 directionAwayFromPlayer;

    
    private float minX = -5.0f;
    private float maxX = 5.0f;
    private float minY = -5.0f;
    private float maxY = 5.0f;


    [Header ("Target")]
    public Vector3 targetPosition;
    private Vector2Int targetPos;

    private bool isWall;

    private enum NPCState { Chase, Flee, Wander, Battle, Run , Idle}
    private NPCState currentState;

    [Header("Distance")]
    public float chaseDistance = 10.0f;  
    public float fleeDistance = 5.0f;    
    public float wanderDistance = 15.0f; 

    private bool canTalk;

    float distanceToTarget;

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        currentState = NPCState.Idle;

        StartCoroutine("FindPath");
    }

    //UPDATE ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Update()
    {
        FindWall();

        if (GetComponent<NPCInfo>().health < GetComponent<NPCInfo>().maxHealth / 2)
        {
            currentState = NPCState.Run;
        }
        else
        {
            if (GetComponent<NPCInfo>().target != null)
            {
                if (currentState != NPCState.Battle)
                {
                    ChangeState();
                }
            }
        }

        Behave();

        MoveToTarget();
        

    }


    void FindWall()
    {
        isWall = false;
        for (int i = 0; i < walls.Length; i++)
        {
            if (Vector3.Distance(targetPosition, walls[i].transform.position) < 1.0f)
            {
                isWall = true;
                break;
            }
        }

        if (!isWall)
        {
            targetPos = new Vector2Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.y));
        }

    }

    //NPC ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void ChangeState()
    {
        if (GetComponent<NPCInfo>().target != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, GetComponent<NPCInfo>().target.transform.position);
        }
        if (GetComponent<NPCInfo>().isBattle)
        {
            if (distanceToTarget < fleeDistance)
            {
                currentState = NPCState.Flee;
            }
            else if (distanceToTarget < chaseDistance)
            {
                currentState = NPCState.Chase;
            }
            else if (distanceToTarget > wanderDistance)
            {
                currentState = NPCState.Wander;
            }
            canTalk = false;
        }

        if (!GetComponent<NPCInfo>().isBattle)
        {
            currentState = NPCState.Idle;
            canTalk = true;
        }
    }

    void Behave()
    {
        switch (currentState)
        {
            case NPCState.Chase:
                Chase();
                break;

            case NPCState.Flee:
                Flee();
                break;

            case NPCState.Wander:
                Wander();
                break;

            case NPCState.Battle:
                Battle();
                break;

            case NPCState.Run:
                Run(); 
                break;

            case NPCState.Idle:
                Idle();
                break;

        }
    }

    //NPC - Battle ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Chase()
    {
        if (GetComponent<NPCInfo>().target != null)
        {
            targetPosition = GetComponent<NPCInfo>().target.transform.position;
        }
    }
    void Flee()
    {
        if (GetComponent<NPCInfo>().target != null)
        {
            directionAwayFromPlayer = (transform.position - GetComponent<NPCInfo>().target.transform.position).normalized;
        }
        targetPosition = transform.position + directionAwayFromPlayer * Random.Range(3.0f, 7.0f);
    }
    void Wander()
    {
        targetPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            transform.position.z
        );
    }
    void Battle()
    {
        targetPosition = new Vector3(
            Random.Range(transform.position.x + minX, transform.position.x + maxX),
            Random.Range(transform.position.y + minY, transform.position.y + maxY),
            transform.position.z
        );
    }
    void Run()
    {
        targetPosition = GameObject.FindWithTag("Run").transform.position;
    }

    //NPC - Idle -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Idle()
    {
        targetPosition = new Vector3(
          Random.Range(minX, maxX),
          Random.Range(minY, maxY),
          transform.position.z
        );
    }

    //Ray ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void MoveToTarget()
    {
        if (GetComponent<MoveManager>().FinalNodeList.Count > 1)
        {
            Vector2 targetPosition = new Vector2(GetComponent<MoveManager>().FinalNodeList[1].x, GetComponent<MoveManager>().FinalNodeList[1].y);

            if (GetComponent<NPCInfo>().target != null)
            {
                Up();
                Down();
                Left();
                Right();
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 4 * Time.deltaTime);

            // 목표 지점에 도달하면 리스트에서 해당 노드를 제거
            if ((Vector2)transform.position == targetPosition)
            {
                GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
            }
        }
    }
    void Up()
    {
        RaycastHit2D[] hitup = Physics2D.RaycastAll(transform.position, Vector2.up);
        for (int i = 0; i < hitup.Length; i++)
        {
            if (hitup[i].transform != null)
            {
                if (hitup[i].distance < 0.5f && hitup[i].collider.CompareTag("Wall"))
                {
                    GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                }

                if (currentState == NPCState.Chase)
                {

                    if (Vector2.Distance(transform.position, GetComponent<NPCInfo>().target.transform.position) < GetComponent<NPCInfo>().attackRange)
                    {
                        GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                        currentState = NPCState.Battle;

                    }

                }
            }
        }
    }
    void Down()
    {
        RaycastHit2D[] hitdown = Physics2D.RaycastAll(transform.position, Vector2.down);

        for (int i = 0; i < hitdown.Length; i++)
        {
            if (hitdown[i].transform != null)
            {
                if (hitdown[i].distance < 0.5f && hitdown[i].collider.CompareTag("Wall"))
                {
                    GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                }

                if (currentState == NPCState.Chase)
                {

                    if (Vector2.Distance(transform.position, GetComponent<NPCInfo>().target.transform.position) < GetComponent<NPCInfo>().attackRange)
                    {
                        GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                        currentState = NPCState.Battle;

                    }

                }

            }
        }
    }
    void Left()
    {
        RaycastHit2D[] hitleft = Physics2D.RaycastAll(transform.position, Vector2.left);
        for (int i = 0; i < hitleft.Length; i++)
        {
            if (hitleft[i].transform != null)
            {
                if (hitleft[i].distance < 0.5f && hitleft[i].collider.CompareTag("Wall"))
                {
                    GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                }

                if (currentState == NPCState.Chase)
                {

                    if (Vector2.Distance(transform.position, GetComponent<NPCInfo>().target.transform.position) < GetComponent<NPCInfo>().attackRange)
                    {
                        GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                        currentState = NPCState.Battle;

                    }

                }

            }
        }
    }
    void Right()
    {
        RaycastHit2D[] hitright = Physics2D.RaycastAll(transform.position, Vector2.right);
        for (int i = 0; i < hitright.Length; i++)
        {
            if (hitright[i].transform != null)
            {
                if (hitright[i].distance < 0.5f && hitright[i].collider.CompareTag("Wall"))
                {
                    GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                }

                if (currentState == NPCState.Chase)
                {

                    if (Vector2.Distance(transform.position, GetComponent<NPCInfo>().target.transform.position) < GetComponent<NPCInfo>().attackRange)
                    {
                        GetComponent<MoveManager>().FinalNodeList.RemoveAt(0);
                        currentState = NPCState.Battle;

                    }

                }
            }
        }
    }


    //Coroutine ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator FindPath()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.8f);

            GetComponent<MoveManager>().PathFindingToTargetPos(targetPos);
            
        }
    }


}
