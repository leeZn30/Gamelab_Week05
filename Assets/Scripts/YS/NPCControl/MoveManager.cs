using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }


    public bool isWall;
    public Node ParentNode;

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}


public class MoveManager : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;

    public bool spinUp;
    public bool spinDown;
    public bool spinLeft;
    public bool spinRight;

    int sizeX, sizeY;
    public Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public int point;

    //�÷��̾� ��ġ������ �� ã��
    public void PathFindingToPlayer(Vector2Int pos)
    {

        GameObject player = GameObject.FindWithTag("Player");


        startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        if (Vector3.Distance(transform.position, player.transform.position) < 30)
        {
            targetPos = new Vector2Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y));
        }
        else
        {
            targetPos = pos;
        }

        topRight.x = startPos.x + 20;
        topRight.y = startPos.y + 20;

        bottomLeft.x = startPos.x - 20;
        bottomLeft.y = startPos.y - 20;


        // NodeArray�� ũ�� �����ְ�, isWall, x, y ����

        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }


        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        if (Vector2.Distance(startPos, targetPos) < 20)
        {
            TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

            OpenList = new List<Node>() { StartNode };
            ClosedList = new List<Node>();
            FinalNodeList = new List<Node>();

            float distance = Vector2.Distance(new Vector2(StartNode.x, StartNode.y), new Vector2(TargetNode.x, TargetNode.y));
            if (distance < 20)
            {
                while (OpenList.Count > 0)
                {
                    // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
                    CurNode = OpenList[0];
                    for (int i = 1; i < OpenList.Count; i++)
                        if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

                    OpenList.Remove(CurNode);
                    ClosedList.Add(CurNode);

                    // ������
                    if (CurNode == TargetNode)
                    {

                        Node TargetCurNode = TargetNode;
                        while (TargetCurNode != StartNode)
                        {
                            FinalNodeList.Add(TargetCurNode);
                            TargetCurNode = TargetCurNode.ParentNode;
                        }
                        FinalNodeList.Add(StartNode);
                        FinalNodeList.Reverse();

                        return;
                    }


                    // �֢آע�
                    if (allowDiagonal)
                    {
                        OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                        OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                        OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                        OpenListAdd(CurNode.x + 1, CurNode.y - 1);
                    }

                    // �� �� �� ��
                    OpenListAdd(CurNode.x, CurNode.y + 1);
                    OpenListAdd(CurNode.x + 1, CurNode.y);
                    OpenListAdd(CurNode.x, CurNode.y - 1);
                    OpenListAdd(CurNode.x - 1, CurNode.y);
                }

            }


        }
    }

    //������ġ�� ���� �� ã��
    public void PathFindingToTargetPos(Vector2Int pos)
    {

        startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        targetPos = pos;


        topRight.x = startPos.x + 20;
        topRight.y = startPos.y + 20;

        bottomLeft.x = startPos.x - 20;
        bottomLeft.y = startPos.y - 20;


        // NodeArray�� ũ�� �����ְ�, isWall, x, y ����

        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }


        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        if (Vector2.Distance(startPos, targetPos) < 20)
        {
            TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

            OpenList = new List<Node>() { StartNode };
            ClosedList = new List<Node>();
            FinalNodeList = new List<Node>();

            float distance = Vector2.Distance(new Vector2(StartNode.x, StartNode.y), new Vector2(TargetNode.x, TargetNode.y));
            
            while (OpenList.Count > 0)
            {
                // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
                CurNode = OpenList[0];
                for (int i = 1; i < OpenList.Count; i++)
                    if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

                OpenList.Remove(CurNode);
                ClosedList.Add(CurNode);

                // ������
                if (CurNode == TargetNode)
                {

                    Node TargetCurNode = TargetNode;
                    while (TargetCurNode != StartNode)
                    {
                        FinalNodeList.Add(TargetCurNode);
                        TargetCurNode = TargetCurNode.ParentNode;
                    }
                    FinalNodeList.Add(StartNode);
                    FinalNodeList.Reverse();

                    return;
                }


                // �֢آע�
                if (allowDiagonal)
                {
                    OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                    OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                    OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                    OpenListAdd(CurNode.x + 1, CurNode.y - 1);
                }

                // �� �� �� ��
                OpenListAdd(CurNode.x, CurNode.y + 1);
                OpenListAdd(CurNode.x + 1, CurNode.y);
                OpenListAdd(CurNode.x, CurNode.y - 1);
                OpenListAdd(CurNode.x - 1, CurNode.y);
            }
        }
    }


    void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // �밢�� ����, �� ���̷� ��� �ȵ�
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // �ڳʸ� �������� ���� ������, �̵� �߿� �������� ��ֹ��� ������ �ȵ�
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }



    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }


    public void MoveToTarget()
    {
        if (FinalNodeList.Count > 1)
        {
            Vector2 targetPosition = new Vector2(FinalNodeList[1].x, FinalNodeList[1].y);


            RaycastHit2D[] hitdown = Physics2D.RaycastAll(transform.position, Vector2.down);

            for (int i = 0; i < hitdown.Length; i++)
            {
                if (hitdown[i].transform != null)
                {
                    if (hitdown[i].distance < 0.3f && hitdown[i].collider.CompareTag("Wall"))
                    {
                        FinalNodeList.RemoveAt(0);
                    }

                }
            }

            RaycastHit2D[] hitup = Physics2D.RaycastAll(transform.position, Vector2.up);
            for (int i = 0; i < hitup.Length; i++)
            {
                if (hitup[i].transform != null)
                {
                    if (hitup[i].distance < 0.3f && hitup[i].collider.CompareTag("Wall"))
                    {
                        FinalNodeList.RemoveAt(0);
                    }
                }
            }

            RaycastHit2D[] hitleft = Physics2D.RaycastAll(transform.position, Vector2.left);
            for (int i = 0; i < hitleft.Length; i++)
            {
                if (hitleft[i].transform != null)
                {
                    if (hitleft[i].distance < 0.3f && hitleft[i].collider.CompareTag("Wall"))
                    {
                        FinalNodeList.RemoveAt(0);
                    }

                }
            }

            RaycastHit2D[] hitright = Physics2D.RaycastAll(transform.position, Vector2.right);
            for (int i = 0; i < hitright.Length; i++)
            {
                if (hitright[i].transform != null)
                {
                    if (hitright[i].distance < 0.3f && hitright[i].collider.CompareTag("Wall"))
                    {
                        FinalNodeList.RemoveAt(0);
                    }
                }
            }


            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 4 * Time.deltaTime);

            // ��ǥ ������ �����ϸ� ����Ʈ���� �ش� ��带 ����
            if ((Vector2)transform.position == targetPosition)
            {
                FinalNodeList.RemoveAt(0);
            }
        }
    }


}
