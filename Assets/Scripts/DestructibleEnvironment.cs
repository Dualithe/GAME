using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructibleEnvironment : MonoBehaviour
{
    [SerializeField] private int durability;
    [SerializeField] public int structTier;
    [SerializeField] private float dropForce;
    [SerializeField] private Item[] drops;
    [SerializeField] private Text hp;
    private Transform parentOfDrops;
    private bool isAlive = true;


    private void Start()
    {
        parentOfDrops = GameObject.FindWithTag("parentOfDrops").transform;

    }

    public void lowerDurability(int levelOfDice)
    {
        durability -= levelOfDice;
        hp.text = durability.ToString();

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
