using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Reci
{
    public Item result;
    public List<Item> recipe = new List<Item>();

}

public class FurnaceLogic : MonoBehaviour
{
    [SerializeField] private float dropForce;
    [SerializeField] private float anvilRange;
    [SerializeField] private Transform parentOfDrops;
    [SerializeField] private Backpack bp;
    [SerializeField] private List<Reci> recipes;
    [SerializeField] public GameObject hud;
    [SerializeField] public GameObject[] hudItems;
    [SerializeField] public GameObject[] hudItems2;
    [SerializeField] public Sprite hudSprite;
    private List<Item> storedInAnvil = new List<Item>();
    public float playerPos;
    public GameObject player;
    private bool disabled = true;


    private void Start()
    {
        bp = GameObject.FindWithTag("Backpack").GetComponent<Backpack>();
        player = GameObject.FindWithTag("Player");
        parentOfDrops = GameObject.FindWithTag("parentOfDrops").transform;
    }

    private void Update()
    {
        manageHUD();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Item it = collision.gameObject.GetComponent<BulletScript>()?.getItem();
        if (it != null)
        {
            var success = tryAddToAnvil(it);
            if (success)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void dropAnvilItem(Pickup anvilDrop)
    {
        float randomZ = Random.Range(0, 2 * Mathf.PI);
        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        var vec = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        var items = Instantiate(anvilDrop, transform.position, Quaternion.Euler(0, 0, randomZ), parentOfDrops);
        items.GetComponent<Rigidbody2D>().AddForce(vec * dropForce, ForceMode2D.Impulse);
        checkIfHUDUpdated();

    }
    public bool tryAddToAnvil(Item item)
    {
        foreach (Reci rec in recipes)
        {
            bool isGoodRecipe = true;
            for (int i = 0; i < storedInAnvil.Count; i++)
            {
                if (storedInAnvil[i].name != rec.recipe[i].name)
                {
                    isGoodRecipe = false;
                    break;
                }
            }
            //Error
            if (isGoodRecipe)
            {
                if (item.name == rec.recipe[storedInAnvil.Count].name)
                {
                    storedInAnvil.Add(item);

                    if (storedInAnvil.Count == rec.recipe.Count)
                    {
                        var it = ScriptableObject.Instantiate(rec.result);
                        it.Init();
                        it.createPickup(transform.position);
                        storedInAnvil.Clear();
                    }
                    checkIfHUDUpdated();
                    return true;
                }

            }
        }
        return false;
    }

    private void manageHUD()
    {
        playerPos = Vector3.Distance(player.transform.position, transform.position);
        if (playerPos < anvilRange && disabled)
        {
            hud.SetActive(true);
            disabled = false;
        }
        else if (playerPos > anvilRange && !disabled)
        {
            hud.SetActive(false);
            disabled = true;
        }
    }

    private void checkIfHUDUpdated()
    {
        for (int i = 0; i < hudItems.Length; i++)
        {
            if (storedInAnvil.Count / 2 > i)
            {
                hudItems[i].GetComponent<Image>().sprite = storedInAnvil[i].sprite;
                hudItems[i].GetComponent<Image>().color = storedInAnvil[i].color;
            }
            else if (storedInAnvil.Count > i)
            {
                hudItems[i].GetComponent<Image>().sprite = storedInAnvil[i].sprite;
                hudItems[i].GetComponent<Image>().color = storedInAnvil[i].color;
            }
            else
            {
                hudItems[i].GetComponent<Image>().sprite = hudSprite;
                hudItems[i].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
