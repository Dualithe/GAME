using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Item item;
    private Backpack bp;

    private Collider2D cl;
    private void Awake()
    {
        bp = GameObject.FindWithTag("Backpack").GetComponent<Backpack>();
        cl = GetComponent<Collider2D>();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cl.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bool added = bp.AddToEq(item);
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }

    public void setVelocity(Vector2 vec)
    {
        GetComponent<Rigidbody2D>().velocity = vec;
    }

    public void setItem(Item i)
    {
        item = i;
    }
}
