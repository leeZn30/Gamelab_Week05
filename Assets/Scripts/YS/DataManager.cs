using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static DataManager Instance { get; private set; }

    // Private fields for user stats

    private float speed;

    private float attacSpeed;
    private float damage;

    private float health;
    private float maxHealth;

    private float stemina;


    private int bulletCount;
    public bool isDead = false;
    public bool isDectected = false;
    private bool canShoot = false;

    public List<GameObject> homeActiveFire;


    // getset 에 접근하게 해주는 프로퍼티

    public float Stemina
    {
        get { return stemina; }
        set { stemina = value; }
    }

    public bool CanShoot
    {
        get { return canShoot; }
        set { canShoot = value; }
    }

    public bool IsDectected
    {
        get { return isDectected; }
        set { isDectected = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float AttacSpeed
    {
        get { return attacSpeed; }
        set { attacSpeed = value; }
    }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int BulletCount
    {
        get { return bulletCount; }
        set { bulletCount = value; }
    }

    // Optional: Add any additional methods or functionality here

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
