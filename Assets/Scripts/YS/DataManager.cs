using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public struct PlayerSaveStatus
{
    public Vector2 position;
    public Quaternion rotation;
    public float health;
    public int bulletCount;
    public int currentCount;

    public PlayerSaveStatus(Vector2 savePosition, Quaternion saveRotation, float saveHealth, int saveBulletCount, int saveCurrentCount)
    {
        position = savePosition;
        rotation = saveRotation;
        health = saveHealth;
        bulletCount = saveBulletCount;
        currentCount = saveCurrentCount;
    }
}

public class DataManager : MonoBehaviour, IListener
{
    // 싱글턴 인스턴스
    public static DataManager Instance { get; private set; }

    // Private fields for user stats

    private float speed;

    private float attacSpeed;
    private float damage;

    public float health;
    private float maxHealth;

    private int bulletCount;
    public bool isDead = false;
    private bool canShoot = false;
    public string playerState;
    public float ResetCoolDownTime;
    public GameObject player;
    public Shoot shoot;

    public List<GameObject> homeActiveFire;

    void Start()
    {
        EventManager.Instance.AddListener(Event_Type.eSave, this);
        EventManager.Instance.AddListener(Event_Type.eLoad, this);
        player = GameObject.FindWithTag("Player");
    }


    // getset 에 접근하게 해주는 프로퍼티

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
                //GameObject.FindWithTag("Player").GetComponent<PlayerGunManager>().currentGunIndex = 0;
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

    public void OnEvent(Event_Type EventType, Component sender, object Param = null)
    {
            switch (EventType)
            {
                case Event_Type.eSave:
                    SaveManager.Instance.playerSaveStatus.health = health;
                    SaveManager.Instance.playerSaveStatus.bulletCount = shoot.leftAmmo;
                    SaveManager.Instance.playerSaveStatus.currentCount = shoot.currentAmmo;
                    SaveManager.Instance.playerSaveStatus.position = player.transform.position;
                    SaveManager.Instance.playerSaveStatus.rotation = player.transform.rotation;
                break;
                case Event_Type.eLoad:
                    health = SaveManager.Instance.playerSaveStatus.health;
                    bulletCount = SaveManager.Instance.playerSaveStatus.bulletCount;
                    shoot.leftAmmo = SaveManager.Instance.playerSaveStatus.bulletCount;
                    shoot.currentAmmo = SaveManager.Instance.playerSaveStatus.currentCount;
                    shoot.bulletUIManager.SetBulletCount(shoot.currentAmmo);
                    shoot.maxAmmoUI.text = SaveManager.Instance.playerSaveStatus.bulletCount + " / " + shoot.maxAmmo;
                    player.transform.position = SaveManager.Instance.playerSaveStatus.position;
                    player.transform.rotation = SaveManager.Instance.playerSaveStatus.rotation;
                break;
            }
    }
}