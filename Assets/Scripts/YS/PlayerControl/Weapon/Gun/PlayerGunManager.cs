using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{

    public List<GameObject> playerGun;
    public int currentGunIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponChange();
    }

    public void WeaponChange()
    {
        for (int i = 0; i < playerGun.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchGun(i);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (InputManager.Instance.controls.Player.WeaponChange1.WasPerformedThisFrame())
        {
            SwitchGun((currentGunIndex + 1) % playerGun.Count);
        }
        if (InputManager.Instance.controls.Player.WeaponChange2.WasPerformedThisFrame())
        {
            SwitchGun((currentGunIndex - 1 + playerGun.Count) % playerGun.Count);
        }
    }

    void SwitchGun(int index)
    {
        if (index >= 0 && index < playerGun.Count)
        {
            {
                playerGun[currentGunIndex].SetActive(false);
                currentGunIndex = index;
                playerGun[currentGunIndex].SetActive(true);
            }
        }

    }
}
