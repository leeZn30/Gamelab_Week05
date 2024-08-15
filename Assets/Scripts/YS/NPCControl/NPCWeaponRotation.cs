using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWeaponRotation : MonoBehaviour
{

    public float angle;
    public Vector2 aimPos;
    public Vector2 direction;
    public Vector2 targetPos;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = GameObject.FindWithTag("Player").transform.position;
        direction = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y);

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }
}
