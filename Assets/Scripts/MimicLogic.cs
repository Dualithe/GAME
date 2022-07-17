using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class MimicDice
{
    public List<Item> itemList = new List<Item>();

}

public class MimicLogic : MonoBehaviour
{
    [SerializeField] private List<MimicDice> mimicReq;
    [SerializeField] private GameObject[] listOfHUDSprites;
    [SerializeField] public Sprite hudSprite;
    private List<Item> currReq = new List<Item>();

    private void Start()
    {
        var set = Random.Range(0, mimicReq.Count);
        foreach (Item e in mimicReq[set].itemList)
        {
            currReq.Add(e);
        }
        updateHUD();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < currReq.Count; i++)
        {
            if (collision.gameObject.GetComponent<BulletScript>().getItem() == currReq[i])
            {
                currReq.RemoveAt(i);
                updateHUD();
                Destroy(collision.gameObject);
            }
        }
    }

    private void updateHUD()
    {
        for (int i = 0; i < mimicReq[0].itemList.Count; i++)
        {
            if (currReq[i] != null)
            {
                listOfHUDSprites[i].GetComponent<Image>().sprite = currReq[i].sprite;
                listOfHUDSprites[i].GetComponent<Image>().color = currReq[i].color;
            }
            else
            {
                listOfHUDSprites[i].GetComponent<Image>().sprite = hudSprite;
                listOfHUDSprites[i].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
