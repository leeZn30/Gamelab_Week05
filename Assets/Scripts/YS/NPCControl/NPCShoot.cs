using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject rotation;

    private bool shooting;

    public float shootDelay;
    private float shootTime;

    private void Start()
    {
        rotation = gameObject.transform.parent.parent.gameObject;
        StartCoroutine(ShootDelay());
    }
    // Update is called once per frame
    void Update()
    {
        if (shootTime > shootDelay)
        {
            shootTime = 0;
            Instantiate(bullet, transform.position, rotation.transform.rotation);
        }
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
