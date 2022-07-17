using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "GAME/Item")]
public class Item : ScriptableObject
{
    [SerializeField] public BulletScript bulletPrefab;
    [SerializeField] public Pickup pickupPrefab;
    [SerializeField] public int maxDurability;
    [SerializeField] public float dropForce;
    [SerializeField] public int tier;
    public string name;
    private int durability;
    public Sprite sprite;
    public Color color;

    public void Init()
    {
        durability = maxDurability;

    }

    public void useItem(Vector3 pos)
    {
        var endPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        var diceBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        diceBullet.GetComponent<BulletScript>().setDirection(new Vector2(endPos.x, endPos.y) - (Vector2)pos);
        diceBullet.setItem(this);
    }

    public Pickup createPickup(Vector3 pos)
    {
        var endPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        var dicePickup = Instantiate(pickupPrefab, pos, Quaternion.identity);
        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        var vec = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        dicePickup.GetComponent<Rigidbody2D>().AddForce(vec * dropForce, ForceMode2D.Impulse);
        dicePickup.setItem(this);
        return dicePickup;
    }

    public void subbDurability()
    {
        durability--;
    }

    public bool isAlive()
    {
        return durability>0;
    }
}
