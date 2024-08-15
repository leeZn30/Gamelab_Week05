using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Transform playerTransform;
    private GameObject[] walls;

    private float minX = -5.0f;
    private float maxX = 5.0f;
    private float minY = -5.0f;
    private float maxY = 5.0f;

    private Vector2Int targetPos;
    private Vector3 targetPosition;

    private bool isWall;

    private enum NPCState { Chase, Flee, Wander }
    private NPCState currentState;

    public float chaseDistance = 10.0f;  // �Ѿư��� �Ÿ�
    public float fleeDistance = 5.0f;    // �������� �Ÿ�
    public float wanderDistance = 15.0f; // ���� �̵� �Ÿ�

    private bool canTalk;

    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = NPCState.Wander; // �⺻ ���¸� Wander(���� �̵�)�� ����
        StartCoroutine("FindPath");
    }

    //UPDATE ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Update()
    {
        isWall = false;

        // NPC ���� ����
        ChangeState();

        // ���¿� ���� �ൿ
        Behave();

        // ������ �浹 üũ
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


        if (targetPos != null && !GetComponent<Talking>().isTalking)
        {
            GetComponent<MoveManager>().MoveToTarget();
        }

    }

    //NPC ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void ChangeState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);


        if (distanceToPlayer < fleeDistance)
        {
            currentState = NPCState.Flee;
            canTalk = false;
        }
        else if (distanceToPlayer < chaseDistance)
        {
            currentState = NPCState.Chase;
            canTalk = false;
        }
        else if (distanceToPlayer > wanderDistance)
        {
            currentState = NPCState.Wander;
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
                // ���� ��ġ�� ����
                Wander();
                break;
        }
    }
    void Chase()
    {
        targetPosition = playerTransform.position;  // �÷��̾ ��ǥ�� ����
    }
    void Flee()
    {
        Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
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

    //Coroutine ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator FindPath()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.8f);

            if (currentState != NPCState.Wander)
            {
                GetComponent<MoveManager>().PathFindingToTargetPos(targetPos);
            }

            Debug.Log("��� ã��");
        }
    }
}
