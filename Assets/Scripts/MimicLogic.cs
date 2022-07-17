using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimicLogic : MonoBehaviour
{

    [SerializeField] private GameObject[] listOfHUDSprites;
    [SerializeField] public Sprite hudSprite;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject hud;
    [SerializeField] public float mimicRange;
    public ChunkGenData chunkToUnlock;
    public List<Item> requiredItems;
    public Vector2 chunkIdx;
    public GridGenerate gen;
    private bool disabled;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        gen = GameObject.FindWithTag("Generator").GetComponent<GridGenerate>();
        updateHUD();
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
            var success = tryAddToMimic(it);
            if (success)
            {
                Destroy(collision.gameObject);
            }
            updateHUD();
        }
    }

    private bool tryAddToMimic(Item item)
    {
        if (chunkToUnlock.reqItems.Count == 0)
        {
            gen.buyLand((int)chunkIdx.x, (int)chunkIdx.y, chunkToUnlock);
            Destroy(gameObject);
            return true;
        }
        if (item.name == chunkToUnlock.reqItems[requiredItems.Count].name)
        {
            requiredItems.Add(item);

            if (requiredItems.Count == chunkToUnlock.reqItems.Count)
            {
                gen.buyLand((int)chunkIdx.x, (int)chunkIdx.y, chunkToUnlock);
                Destroy(gameObject);
            }
            return true;
        }
        return false;
    }


    private void updateHUD()
    {
        for (int i = 0; i < listOfHUDSprites.Length; i++)
        {
            if (i < requiredItems.Count)
            {
                listOfHUDSprites[i].GetComponent<Image>().sprite = chunkToUnlock.reqItems[i].sprite;
                listOfHUDSprites[i].GetComponent<Image>().color = chunkToUnlock.reqItems[i].color;
            }
            else
            {
                listOfHUDSprites[i].GetComponent<Image>().sprite = hudSprite;
                listOfHUDSprites[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    private void manageHUD()
    {
        var playerPos = Vector3.Distance(player.transform.position, transform.position);
        if (playerPos < mimicRange && disabled)
        {
            hud.SetActive(true);
            disabled = false;
        }
        else if (playerPos > mimicRange && !disabled)
        {
            hud.SetActive(false);
            disabled = true;
        }
    }
}
