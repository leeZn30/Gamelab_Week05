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

    private GameObject battleUI;

    // Start is called before the first frame update
    void Start()
    {
        PlayerGunSetting();
    }

    // Update is called once per frame
    void Update()
    {
        battleUI = GameObject.FindWithTag("Canvas").transform.Find("BattleUI").gameObject;
        healthUIManager = GameObject.FindWithTag("Canvas").transform.Find("BattleUI").transform.GetComponentInChildren<HealthUIManager>();
        PlayerGunUI = battleUI.transform.Find("SkillCoolDown").gameObject;

        if (DataManager.Instance.playerState == "Battle")
        {
            if (battleUI != null)
            {
                battleUI.SetActive(true);
                healthUIManager.SethealthCount(DataManager.Instance.Health);
            }
        }

        if (DataManager.Instance.playerState != "Battle")
        {
            if (battleUI != null)
            {
                battleUI.SetActive(false);
            }
        }
    }


    void PlayerGunSetting()
    {
        for (int i = 0; i < GetComponent<PlayerGunManager>().playerGun.Count; i++)
        {
            GetComponent<PlayerGunManager>().playerGun[i].SetActive(false);
        }

        GetComponent<PlayerGunManager>().playerGun[GetComponent<PlayerGunManager>().currentGunIndex].SetActive(true);

    }
    void GunUI()
    {
        PlayerGunUI.SetActive(true);
        gunIndexText.text = GetComponent<PlayerGunManager>().currentGunIndex.ToString();
    }
}
