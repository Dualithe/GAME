using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    private List<Item> eq = new List<Item>();
    [SerializeField] private GameObject uiBackpack;
    [SerializeField] private Transform parentOfDrops;
    [SerializeField] private new List<Text> children;
    [SerializeField] private GameObject starterDice;
    private int hovered = 0;
    private Item it;

    private void Start()
    {
        var itm = new Item();
        itm.name = "Starter Dice";
        itm.ammount = 1;
        itm.bullet = starterDice;
        eq.Add(itm);
        Mathf.Clamp(hovered, 0, 5);
        var alpha = uiBackpack.transform.GetChild(0).GetComponent<Image>().color;
        alpha.a += 0.25f;
        uiBackpack.transform.GetChild(hovered).GetComponent<Image>().color = alpha;

        it = uiBackpack.GetComponentInChildren<Item>();

    }

    public bool AddToEq(string nameOfItem)
    {
        if (eq.Count < 6)
        {
            var it = new Item();
            it.name = nameOfItem;
            eq.Add(it);
            children[eq.Count].text = nameOfItem;
            children[eq.Count].text = nameOfItem;
            return true;
        }
        else return false;
    }


    private void Update()
    {
        if (Input.GetKeyDown("q") && hovered > 0)
        {
            changeHovered(-1);
        }
        else if (Input.GetKeyDown("e") && hovered < 5)
        {
            changeHovered(1);
        }
    }

    private void changeHovered(int i)
    {
        int noLongerHovered = hovered;
        hovered += i;

        var x = uiBackpack.transform.GetChild(noLongerHovered).GetComponent<Image>().color;
        x.a -= 0.25f;
        uiBackpack.transform.GetChild(noLongerHovered).GetComponent<Image>().color = x;

        var y = uiBackpack.transform.GetChild(hovered).GetComponent<Image>().color;
        y.a += 0.25f;
        uiBackpack.transform.GetChild(hovered).GetComponent<Image>().color = y;
    }


    public GameObject getHoveredItem()
    {
        return eq[hovered].bullet;
    }

}
