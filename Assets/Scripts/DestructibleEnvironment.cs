using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleEnvironment : MonoBehaviour
{
    [SerializeField] private int durability;
    [SerializeField] public int structTier;
    [SerializeField] private float dropForce;
    [SerializeField] private Item[] drops;
    private Transform parentOfDrops;
    private bool isAlive = true;


    private void Start()
    {
        parentOfDrops = GameObject.FindWithTag("parentOfDrops").transform;

    }

    public void lowerDurability(int levelOfDice)
    {
        durability -= levelOfDice;
        if (durability <= 0 && isAlive)
        {

            dropItems();
            Destroy(gameObject);
            isAlive = false;
        }
    }
    public void dropItems()
    {
        foreach (var drop in drops)
        {
            var item = ScriptableObject.Instantiate(drop);
            item.Init();
            item.createPickup(transform.position);  
        }
    }

}
