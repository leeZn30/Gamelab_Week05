using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static DataManager Instance { get; private set; }

    // Private fields for user stats

    private float speed;

    private float attacSpeed;
    private float damage;

    public float health;
    private float maxHealth;

    private float stemina;
    public float money;

    private int bulletCount;
    public bool isDead = false;
    private bool canShoot = false;
    public string playerState;
    public float ResetCoolDownTime;

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
        }
        else
        {
            Destroy(gameObject);
        }

        playerState = "Idle";
        StartCoroutine(ResetCoolDown());
    }


    private void Update()
    {
        ResetState();
    }

    void ResetState()
    {
        if (ResetCoolDownTime > 5f)
        {
            playerState = "Idle";
            ResetCoolDownTime = 0;

            for(int i = 0; i < GameObject.FindWithTag("Player").GetComponent<PlayerGunManager>().playerGun.Count; i++)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerGunManager>().playerGun[i].SetActive(false);
                GameObject.FindWithTag("Player").GetComponent<PlayerGunManager>().currentGunIndex = 0;
                GameObject.FindWithTag("Player").GetComponent<PlayerGunManager>().isActive = false;
            }
            GameObject.FindWithTag("Player").GetComponent<PlayerUI>().battleUI.SetActive(false);

        }
    }

    IEnumerator ResetCoolDown()
    {
        while (true)
        {
            yield return null;
            ResetCoolDownTime = ResetCoolDownTime + Time.deltaTime;

        }
    }

}
