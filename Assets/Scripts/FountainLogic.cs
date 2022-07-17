using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainLogic : MonoBehaviour
{
    [SerializeField] private float fountainCD;
    [SerializeField] private float dropForce;
    [SerializeField] private Item fountainDrop;
    [SerializeField] private Transform parentOfDrops;
    private float CD;

    private void Start()
    {
        CD = fountainCD;
        parentOfDrops = GameObject.FindWithTag("parentOfDrops").transform;
    }

    private void Update()
    {
        CD -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CD <= 0)
        {

            float randomZ = Random.Range(0, 2 * Mathf.PI);
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            var vec = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            var items = Instantiate(fountainDrop, transform.position, Quaternion.Euler(0, 0, randomZ), parentOfDrops);
            items.createPickup(transform.position);

            CD = fountainCD;
        }
    }
}
