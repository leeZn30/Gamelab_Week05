using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public HealthUIManager healthUIManager;
    //public GameObject stemina;
    public GameObject[] PlayerGunUI;
    public TextMeshProUGUI gunIndexText;


    // Start is called before the first frame update
    void Start()
    {
        PlayerGunSetting();
    }

    // Update is called once per frame
    void Update()
    {
        healthUIManager.SethealthCount(DataManager.Instance.Health);
    }


    void PlayerGunSetting()
    {
        for (int i = 0; i < GetComponent<PlayerGunManager>().playerGun.Count; i++)
        {
            GetComponent<PlayerGunManager>().playerGun[i].SetActive(false);
        }

        GetComponent<PlayerGunManager>().playerGun[GetComponent<PlayerGunManager>().currentGunIndex].SetActive(true);

        PlayerGunUI = new GameObject[10];

        for (int i = 0; i < GameObject.Find("Battle_Ui").transform.Find("SkillCoolDown").transform.childCount; i++)
        {
            PlayerGunUI[i] = GameObject.Find("Battle_Ui").transform.Find("SkillCoolDown").transform.GetChild(i).gameObject;

            if (PlayerGunUI[i].name.Contains("Text"))
            {
                gunIndexText = PlayerGunUI[i].transform.GetComponent<TextMeshProUGUI>();
            }
        }

    }
    void GunUI()
    {
        for (int i = 0; i < PlayerGunUI.Length; i++)
        {
            if (PlayerGunUI[i] != null)
            {
                PlayerGunUI[i].SetActive(true);
            }
            else
            {
                break;
            }
        }

        gunIndexText.text = GetComponent<PlayerGunManager>().currentGunIndex.ToString();
    }
}
