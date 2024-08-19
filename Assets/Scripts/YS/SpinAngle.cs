using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAngle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.parent.GetComponent<NPCInfo>().isBattle)
        {
            if (transform.parent.GetComponent<NPCMovement>().spinDown)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (transform.parent.GetComponent<NPCMovement>().spinUp)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (transform.parent.GetComponent<NPCMovement>().spinLeft)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.parent.GetComponent<NPCMovement>().spinRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }

        }
    }
}
