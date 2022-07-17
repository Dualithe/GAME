using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookLogic : MonoBehaviour
{
    [SerializeField] private Item diceToEnchant;
    [SerializeField] private Item enchantedDicePrefab;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GridGenerate generator;

    private void Start()
    {
        generator = GameObject.FindWithTag("Generator").GetComponent<GridGenerate>();
        spawnFire();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Item it = collision.gameObject.GetComponent<BulletScript>()?.getItem();
        if (it != null)
        {
            var success = tryAddToBook(it);
            if (success)
            {
                Destroy(collision.gameObject);
            }
        }
    }
    private bool tryAddToBook(Item it)
    {
        if (it == diceToEnchant)
        {
            var itm = ScriptableObject.Instantiate(enchantedDicePrefab);
            itm.Init();
            itm.createPickup(transform.position);
            return true;
        }
        return false;
    }

    private void spawnFire()
    {
        Vector2[] coords = {new Vector2(0,1),
new Vector2(0,2),
new Vector2(0,3),

new Vector2(0,-1),
new Vector2(0,-2),
new Vector2(0,-3),

new Vector2(1,0),
new Vector2(2,0),
new Vector2(3,0),

new Vector2(-1,0),
new Vector2(-2,0),
new Vector2(-3,0),

new Vector2(3,1),
new Vector2(1,3),
new Vector2(2,1),
new Vector2(1,1),
new Vector2(1,2),
new Vector2(2,2),

new Vector2(-3,1),
new Vector2(-1,3),
new Vector2(-2,1),
new Vector2(-1,1),
new Vector2(-1,2),
new Vector2(-2,2),

new Vector2(3,-1),
new Vector2(1,-3),
new Vector2(2,-1),
new Vector2(1,-1),
new Vector2(1,-2),
new Vector2(2,-2),

new Vector2(-3,-1),
new Vector2(-1,-3),
new Vector2(-2,-1),
new Vector2(-1,-1),
new Vector2(-1,-2),
new Vector2(-2,-2)
        };
       Vector2 x = generator.getTileSize();

        foreach(Vector2 coord in coords)
        {
            Instantiate(firePrefab, (Vector2)transform.position + (x * coord), Quaternion.identity);
        }

    }
}
