using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private string name;
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
        if (collision.gameObject.CompareTag("player"))
        {
            bool added = bp.AddToEq(name);
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}
