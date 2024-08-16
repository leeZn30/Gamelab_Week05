using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Assasin : MonoBehaviour
{
    public List<GameObject> objectsInCollider = new List<GameObject>();

    private void Update()
    {
        if (InputManager.Instance.controls.Player.Interaction.WasPressedThisFrame())
        {
            if(objectsInCollider.Count > 0)
            {
                for(int i = 0; i < objectsInCollider.Count; i++)
                {
                    objectsInCollider[i].GetComponent<NPCInfo>().health = -1;
                    
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("Cult") || collision.CompareTag("Resistance")) && !objectsInCollider.Contains(collision.gameObject))
        {
            // 콜라이더에 들어온 오브젝트 추가
            objectsInCollider.Add(collision.gameObject);
            Debug.Log(collision.gameObject.name + " entered the collider.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Cult") || collision.CompareTag("Resistance")) && objectsInCollider.Contains(collision.gameObject))
        {
            // 콜라이더에서 나간 오브젝트 제거
            objectsInCollider.Remove(collision.gameObject);
            Debug.Log(collision.gameObject.name + " exited the collider.");
        }
    }

    IEnumerator Assasinate()
    {
        yield return new WaitForSeconds(1f);
    }

}
