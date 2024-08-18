using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public HealthUIManager healthUIManager;
    //public GameObject stemina;
    public GameObject PlayerGunUI;
    public TextMeshProUGUI gunIndexText;

    public GameObject battleUI;

    void Awake()
    {
        battleUI = GameObject.Find("BattleUI");
        healthUIManager = battleUI.GetComponentInChildren<HealthUIManager>();
        PlayerGunUI = battleUI.transform.Find("SkillCoolDown").gameObject;

        battleUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlayerGunSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (InputManager.Instance.controls.Player.Shoot.WasPressedThisFrame())
        {
            if (battleUI != null)
            {
                //battleUI.SetActive(true);
                healthUIManager.SethealthCount(DataManager.Instance.Health);
            }
        }
        

        if (DataManager.Instance.playerState != "Battle")
        {
            if (battleUI != null)
            {
                //battleUI.SetActive(false);
            }
        }
    }

    /*
    void PlayerGunSetting()
    {
        for (int i = 0; i < GetComponent<PlayerGunManager>().playerGun.Count; i++)
        {
            GetComponent<PlayerGunManager>().playerGun[i].SetActive(false);
        }

        // GetComponent<PlayerGunManager>().playerGun[GetComponent<PlayerGunManager>().currentGunIndex].SetActive(true);

    }
    */
    void GunUI()
    {
        PlayerGunUI.SetActive(true);
        gunIndexText.text = GetComponent<PlayerGunManager>().currentGunIndex.ToString();
    }
}
