using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleEnvironment : MonoBehaviour
{
    [SerializeField] private int durability;
    [SerializeField] private float dropForce;
    [SerializeField] private GameObject[] drops;
    [SerializeField] private Transform parentOfDrops;


    private void Start()
    {
        parentOfDrops = GameObject.FindWithTag("parentOfDrops").transform;
    }

    public void lowerDurability()
    {
        durability--;
    }

    private void Update()
    {
        if (durability <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void dropItems(int LevelOfDice)
    {
        float randomZ = Random.Range(0, 2 * Mathf.PI);
        var drop = Random.Range(0, drops.Length);
        for (int i = 0; i < LevelOfDice; i++)
        {
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            var vec = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            var items = Instantiate(drops[drop], transform.position, Quaternion.Euler(0, 0, randomZ), parentOfDrops);
            items.GetComponent<Rigidbody2D>().AddForce(vec * dropForce, ForceMode2D.Impulse);
        }
    }
}
