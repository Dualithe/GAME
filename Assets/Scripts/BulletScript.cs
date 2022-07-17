using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float bulletForce;
    private int levelOfDice;
    private Item item;
    private Vector2 direction;
    private Rigidbody2D rb;

    private List<int> structuresDamaged = new List<int>();

    private Vector2 prevVelocity;

    [SerializeField] private Sprite[] diceSprites;

    private SpriteRenderer spr;

    int diceFrame = 0;

    private void Start()
    {
        rb.AddForce(direction * Random.Range(bulletForce * 0.7f, bulletForce * 0.9f), ForceMode2D.Impulse);
    }

    private void Awake() {        
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        prevVelocity = rb.velocity;
        bulletLifetime -= Time.deltaTime;
        if (bulletLifetime <= 0)
        {
            var pck = item.createPickup(transform.position);
            pck.setVelocity(Vector2.zero);
            pck.GetComponent<Collider2D>().isTrigger = false;
            Destroy(gameObject);
        }
    }
    
    IEnumerator PlayDiceAnimation() {
        diceFrame = 0;
        while (true) {
            diceFrame = (diceFrame + 1) % 6;
            spr.sprite = diceSprites[item.id * 6 + diceFrame];
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void setDirection(Vector2 x)
    {
        direction = x.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector3.Reflect(prevVelocity, collision.GetContact(0).normal) * 0.9f;
        if (collision.gameObject.CompareTag("struct")) {
            var structId = collision.gameObject.GetInstanceID();
            var env = collision.gameObject.GetComponent<DestructibleEnvironment>();
            if (!structuresDamaged.Contains(structId)) {
                structuresDamaged.Add(structId);
                if (item.tier == 0 && env.name.Contains("Fire")) {
                    env.lowerDurability(999999);
                }
                if (item.tier >= env.structTier) {
                    env.lowerDurability(item.value+1);
                    item.subbDurability();
                    if (!item.isAlive()) Destroy(gameObject);
                }
                
            }
        }
    }
    public void setItem(Item i)
    {
        item = i;
        StartCoroutine(PlayDiceAnimation());
    }

    public Item getItem()
    {
        return item;
    }

}
