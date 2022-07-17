using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Item item;
    private Backpack bp;

    private Collider2D cl;

    [SerializeField] private Sprite[] diceSprites;
    [SerializeField] private Sprite[] dicePlateSprites;

    [SerializeField] private SpriteRenderer plate;
    private SpriteRenderer dice;

    private void Awake()
    {
        bp = GameObject.FindWithTag("Backpack").GetComponent<Backpack>();
        cl = GetComponent<Collider2D>();
        dice = GetComponent<SpriteRenderer>();
        cl.isTrigger = false;
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
        // var x = GetComponent<SpriteRenderer>();
        item = i;
        plate.sprite = dicePlateSprites[i.id * 6 + i.value];
        dice.sprite = diceSprites[i.id * 6];
        
        // x.color = item.color;
        // x.sprite = item.sprite;
    }
}
