using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public class Shoot : MonoBehaviour
{
    public int currentAmmo;
    private int leftAmmo;
    public int maxAmmo;

    public GameObject bullet;
    private GameObject rotation;

    private bool first = true;

    private bool shooting;

    public float shootDelay;
    public float shootTime;

    public bool isReloading;

    private BulletUIManager bulletUIManager;

    [SerializeField] TextMeshProUGUI maxAmmoUI;

    private void Start()
    {
        currentAmmo = DataManager.Instance.BulletCount;
        bulletUIManager.SetBulletCount(currentAmmo);
    }

    private void OnEnable()
    {
        DataManager.Instance.BulletCount = 20;
        maxAmmo = DataManager.Instance.BulletCount * 5;
        isReloading = false;
        shooting = false;

        bulletUIManager = GameObject.Find("BattleUI").transform.GetComponentInChildren<BulletUIManager>();
        rotation = GameObject.FindWithTag("Player").transform.Find("PlayerGunRotatePos").gameObject;
        StartCoroutine(ShootDelay());


        if (first)
        {
            leftAmmo = maxAmmo;
            first = false;
        }

        //maxAmmoUI = GameObject.FindWithTag("Canvas").transform.Find("Battle_Ui").transform.Find("MaxBullet_UI").transform.GetComponent<TextMeshProUGUI>();
        maxAmmoUI.text = leftAmmo + " / " + maxAmmo;
    }
    // Update is called once per frame
    void Update()
    {

        ChangeState();
        if (currentAmmo > 0 && !isReloading)
        {
            if (shooting && shootTime > shootDelay)
            {
                currentAmmo--;
                leftAmmo--;

                if (maxAmmoUI != null)
                {
                    maxAmmoUI.text = leftAmmo + " / " + maxAmmo;
                }

                shootTime = 0;
                Instantiate(bullet, transform.position, rotation.transform.rotation);


                bulletUIManager.SetBulletCount(currentAmmo);

                RumbleManager.instance.RumblePulse(0.3f, 1f, 0.1f);
                DataManager.Instance.ResetCoolDownTime = 0;
            }
        }
        Reload();
    }

    void ChangeState()
    {
        if (InputManager.Instance.controls.Player.Shoot.WasPressedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
        {
            DataManager.Instance.playerState = "Battle";
            shooting = true;
        }
        if (InputManager.Instance.controls.Player.Shoot.WasReleasedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
        {
            shooting = false;
        }
    }

    private void Reload()
    {
        if (currentAmmo == 0 && leftAmmo >= DataManager.Instance.BulletCount)
        {
            StartCoroutine("ReloadTime");
        }
        else if (currentAmmo == 0 && leftAmmo < DataManager.Instance.BulletCount)
        {
            StartCoroutine("ReloadTime1");
        }

        if (InputManager.Instance.controls.Player.Reload.WasPressedThisFrame() && leftAmmo >= DataManager.Instance.BulletCount)
        {
            StartCoroutine("ReloadTime");
        }
        else if (InputManager.Instance.controls.Player.Reload.WasPressedThisFrame() && leftAmmo < DataManager.Instance.BulletCount)
        {
            StartCoroutine("ReloadTime1");
        }

    }
    IEnumerator ReloadTime()
    {
        isReloading = true;
        currentAmmo = DataManager.Instance.BulletCount;
        bulletUIManager.SetBulletCount(currentAmmo);

        yield return new WaitForSeconds(1);
        isReloading = false;


    }
    IEnumerator ReloadTime1()
    {
        isReloading = true;
        currentAmmo = leftAmmo;
        bulletUIManager.SetBulletCount(currentAmmo);

        yield return new WaitForSeconds(1);
        isReloading = false;

    }


    IEnumerator ShootDelay()
    {
        while (true)
        {
            yield return null;
            shootTime += Time.deltaTime;
        }
    }


}
