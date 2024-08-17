using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistRumble : MonoBehaviour
{
    public bool hand; 
    AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (hand != transform.parent.GetComponent<FistAttack>().hand)
        {
            if (transform.parent.GetComponent<FistAttack>().isAttacking)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
            if (!transform.parent.GetComponent<FistAttack>().isAttacking)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Cult") || collision.collider.CompareTag("Resistance"))
        {
            audioSource.Play();
            RumbleManager.instance.RumblePulse(1f, 1f, 0.2f);
            
        }       
    }

}
