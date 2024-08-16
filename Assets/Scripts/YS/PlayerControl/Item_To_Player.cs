using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using static UnityEditor.Timeline.Actions.MenuPriority;

public class Item_To_Player : MonoBehaviour
{
    public GameObject relatedObject;
    public int price;
    public GameObject priceText;

    GameObject player;

    bool canGet;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        price = Random.Range(100, 200);
    }

    // Update is called once per frame
    void Update()
    {
        if (canGet && InputManager.Instance.controls.Player.Interaction.WasPerformedThisFrame())
        {
            if(DataManager.Instance.money >= price)
            {
                var obj = Instantiate(relatedObject, player.transform.GetChild(1).GetChild(0));
                player.GetComponent<PlayerGunManager>().playerGun.Add(obj);
                obj.SetActive(false);

                DataManager.Instance.money -= price;

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 들어옴");
            GameObject a = gameObject.transform.GetChild(1).gameObject;
            a.SetActive(true);
            canGet = true;
            
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject a = gameObject.transform.GetChild(1).gameObject;
            a.SetActive(false);
            canGet = false;
        }

    }

}
