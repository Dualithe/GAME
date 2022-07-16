using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float bulletForce;
    [SerializeField] private int levelOfDice;
    private bool firstEntered = false;
    private Vector2 direction;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        bulletLifetime -= Time.deltaTime;
        if (bulletLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void setDirection(Vector2 x)
    {
        direction = x.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!firstEntered)
        {
            rb.velocity = Vector3.Reflect(transform.position, collision.contacts[0].normal) * 0.2f;
            collision.gameObject.GetComponent<DestructibleEnvironment>().lowerDurability();
            collision.gameObject.GetComponent<DestructibleEnvironment>().dropItems(levelOfDice);
            firstEntered = true;
        }
    }

}
