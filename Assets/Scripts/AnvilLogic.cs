using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Recipe
{
    public Item result;
    public List<Item> recipe = new List<Item>();

}

public class AnvilLogic : MonoBehaviour
{
    [SerializeField] private float dropForce;
    [SerializeField] private float anvilRange;
    [SerializeField] private Transform parentOfDrops;
    [SerializeField] private Backpack bp;
    [SerializeField] private List<Recipe> recipes;
    [SerializeField] public GameObject hud;
    [SerializeField] public GameObject enchantedAnvil;
    [SerializeField] public GameObject[] hudItems;
    [SerializeField] private GameObject[] slots;
    [SerializeField] public Sprite hudSprite;
    [SerializeField] private Sprite[] diceSprites;
    private List<Item> storedInAnvil = new List<Item>();
    public float playerPos;
    public GameObject player;
    int diceFrame = 0;

    private void Start()
    {
        bp = GameObject.FindWithTag("Backpack").GetComponent<Backpack>();
        player = GameObject.FindWithTag("Player");
        parentOfDrops = GameObject.FindWithTag("parentOfDrops").transform;
        StartCoroutine(PlayDiceAnimation());
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        hud.SetActive(false);
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
        foreach (Recipe rec in recipes)
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
                    slots[storedInAnvil.Count].transform.GetChild(0).gameObject.SetActive(true);
                    storedInAnvil.Add(item);
                    updateHUD();

                    if (storedInAnvil.Count == rec.recipe.Count)
                    {
                        if (rec.result != null)
                        {
                            var it = ScriptableObject.Instantiate(rec.result);
                            it.Init();
                            it.createPickup(transform.position);
                        }
                        else
                        {
                            foreach (Item itm in storedInAnvil)
                            {
                                var it = ScriptableObject.Instantiate(itm);
                                it.Init();
                                it.createPickup(transform.position);
                            }
                            Instantiate(enchantedAnvil, transform.position, Quaternion.identity);
                            Destroy(gameObject);
                        }
                        storedInAnvil.Clear();
                        for (int i = 0; i < slots.Length; i++)
                        {
                            slots[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
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
        if (playerPos < anvilRange)
        {
            hud.SetActive(true);
        }
        else if (playerPos > anvilRange)
        {
            hud.SetActive(false);
        }
    }

    private void checkIfHUDUpdated()
    {
        for (int i = 0; i < hudItems.Length; i++)
        {
            if (storedInAnvil.Count > i)
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

    private IEnumerator PlayDiceAnimation()
    {
        while (true)
        {
            diceFrame = (diceFrame + 1) % 6;
            updateHUD();
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void updateHUD()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            if (i < storedInAnvil.Count)
            {
                var diceId = storedInAnvil[i].id;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = diceSprites[diceId * 6 + (diceFrame + i) % 6];
            }
        }
    }
}
