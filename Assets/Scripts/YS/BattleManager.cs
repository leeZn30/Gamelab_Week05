using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public List<GameObject> resistance;
    public List<GameObject> cult;

    private List<GameObject> resistanceSave;
    private List<GameObject> cultSave;

    public List<GameObject> Resistance
    {
        get { return resistance; }
        set { resistance = value; }
    }

    public List<GameObject> Cult
    {
        get { return cult; }
        set { cult = value; }
    }


    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }



    public void ResetList()
    {
        resistanceSave = new List<GameObject>();
        cultSave = new List<GameObject>();


        for (int i = 0; i < resistance.Count; i++)
        {
            if(resistance[i] != null)
            {
                resistanceSave.Add(resistance[i]);
            }
        }
        for (int i = 0; i < cult.Count; i++)
        {
            if (resistance[i] != null)
            {
                cultSave.Add(cult[i]);
            }
        }

        resistance = new List<GameObject>();
        resistance = resistanceSave;

        cult = new List<GameObject>();
        cult = cultSave;
    }
}
