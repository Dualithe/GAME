using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float bulletForce;
    [SerializeField] private int levelOfDice;
    private Item item;
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
            var pck = item.createPickup(transform.position);
            pck.setVelocity(Vector2.zero);
            pck.GetComponent<Collider2D>().isTrigger = false;
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
            if (collision.gameObject.CompareTag("struct") && item.tier == 0 && collision.gameObject.GetComponent<DestructibleEnvironment>().name.Contains("Fire"))
            {
                collision.gameObject.GetComponent<DestructibleEnvironment>().lowerDurability(999999);
                if (!item.isAlive()) Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("struct") && item.tier > collision.gameObject.GetComponent<DestructibleEnvironment>().structTier)
            {
                collision.gameObject.GetComponent<DestructibleEnvironment>().lowerDurability(levelOfDice);
                item.subbDurability();
                if (!item.isAlive()) Destroy(gameObject);
            }
            firstEntered = true;
        }
    }
    public void setItem(Item i)
    {
        item = i;
    }

    public Item getItem()
    {
        return item;
    }

}
