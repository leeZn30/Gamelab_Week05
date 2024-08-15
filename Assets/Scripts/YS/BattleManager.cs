using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public List<GameObject> ally;
    public List<GameObject> enemy;

    private List<GameObject> allySave;
    private List<GameObject> enemySave;

    public List<GameObject> Ally
    {
        get { return ally; }
        set { ally = value; }
    }

    public List<GameObject> Enemy
    {
        get { return enemy; }
        set { enemy = value; }
    }


    void Awake()
    {
        // ½Ì±ÛÅæ ÆÐÅÏ ±¸Çö
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        allySave = new List<GameObject>();
        enemySave = new List<GameObject>();
    }



    public void ResetList()
    {
        allySave = new List<GameObject>();
        enemySave = new List<GameObject>();


        for (int i = 0; i < ally.Count; i++)
        {
            if(ally[i] != null)
            {
                allySave.Add(ally[i]);
            }
        }
        for (int i = 0; i < enemy.Count; i++)
        {
            if (ally[i] != null)
            {
                enemySave.Add(enemy[i]);
            }
        }

        ally = new List<GameObject>();
        ally = allySave;

        enemy = new List<GameObject>();
        enemy = enemySave;
    }
}
