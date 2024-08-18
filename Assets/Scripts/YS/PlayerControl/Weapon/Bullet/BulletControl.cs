using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    public AudioSource AudioSource;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();

        AudioSource.Play();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Cult") || collision.collider.CompareTag("Resistance") || collision.collider.CompareTag("Statue"))
        {
            Destroy(gameObject);
        }
    }

}
