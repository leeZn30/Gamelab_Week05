using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    private GameObject rotation;

    private bool shooting;

    public float shootDelay;
    private float shootTime;

    private void Start()
    {
        rotation = GameObject.FindWithTag("Player").transform.Find("PlayerGunRotatePos").gameObject;
        StartCoroutine(ShootDelay());
    }
    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.controls.Player.Shoot.WasPressedThisFrame())
        {
            shooting = true;
        }
        if (InputManager.Instance.controls.Player.Shoot.WasReleasedThisFrame())
        {
            shooting = false;
        }

        if (shooting && shootTime > shootDelay)
        {
            shootTime = 0;
            Instantiate(bullet, transform.position, rotation.transform.rotation);
            RumbleManager.instance.RumblePulse(0.5f,0.5f,0.1f);
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
