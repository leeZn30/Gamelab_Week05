using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCWeaponRotation : MonoBehaviour
{

    public float angle;
    public Vector2 aimPos;
    public Vector2 direction;


    public Vector2 targetPos;

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<NPCInfo>().target != null)
        {
            targetPos = transform.parent.GetComponent<NPCInfo>().target.transform.position;
        }
        aimPos = transform.parent.transform.position;


        direction = new Vector2(targetPos.x - aimPos.x, targetPos.y - aimPos.y);
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }


 

}
