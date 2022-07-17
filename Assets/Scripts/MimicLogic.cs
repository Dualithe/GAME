using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MimicLogic : MonoBehaviour
{

    [SerializeField] private GameObject[] slots;
    [SerializeField] public Sprite hudSprite;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject hud;
    [SerializeField] public float mimicRange;
    public ChunkGenData chunkToUnlock;
    public List<Item> requiredItems;
    public Vector2 chunkIdx;
    public GridGenerate gen;
    private bool disabled;

    [SerializeField] private Sprite[] diceSprites;

    int diceFrame = 0;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        gen = GameObject.FindWithTag("Generator").GetComponent<GridGenerate>();
        updateHUD();
    }

    public void SetChunkToUnlock(ChunkGenData chunk) {
        chunkToUnlock = chunk;
        StartCoroutine(PlayDiceAnimation());
    }

    private IEnumerator PlayDiceAnimation() {
        while(true) {
            diceFrame = (diceFrame + 1) % 6;
            for (int i = 0; i < slots.Length; ++i) {
                if (i >= requiredItems.Count) {
                    var diceId = chunkToUnlock.reqItems[i].id;
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = diceSprites[diceId * 6 + (diceFrame + i) % 6];
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
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
            var renderer = slots[requiredItems.Count].transform.GetChild(0).GetComponent<Image>();
            renderer.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
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
        // for (int i = 0; i < slots.Length; i++)
        // {
        //     var diceId = chunkToUnlock.reqItems[i].id;
        //     if (i < requiredItems.Count)
        //     {
        //         slots[i].GetComponent<Image>().sprite = diceSprites[diceId * 6];
        //         // slots[i].GetComponent<Image>().color = chunkToUnlock.reqItems[i].color;
        //     }
        //     else
        //     {
        //         slots[i].GetComponent<Image>().sprite = hudSprite;
        //         // slots[i].GetComponent<Image>().color = Color.white;
        //     }
        // }
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
