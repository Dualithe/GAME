using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    [SerializeField] public List<Item> eq = new List<Item>();
    [SerializeField] private GameObject uiBackpack;
    [SerializeField] private Transform parentOfDrops;
    [SerializeField] private new List<Text> children;
    private int hovered = 0;

    private void Start()
    {
        for (int i = 0; i < eq.Count; i++)
        {
            eq[i] = ScriptableObject.Instantiate(eq[i]);
            eq[i].Init();
        }

        var alpha = uiBackpack.transform.GetChild(0).GetComponent<Image>().color;
        alpha.a += 0.25f;
        uiBackpack.transform.GetChild(hovered).GetComponent<Image>().color = alpha;

        updateHUD();
    }

    public bool AddToEq(Item item)
    {
        if (eq.Count < 6)
        {
            eq.Add(item);
            updateHUD();
            return true;
        }
        else return false;
    }

    public void RemoveHoveredFromEq()
    {
        eq.RemoveAt(hovered);
        updateHUD();

    }

    public void updateHUD()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (i < eq.Count)
            {
                children[i].text = eq[i].name;
            }
            else children[i].text = "X";
        }
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
        hovered = Mathf.Clamp(hovered, 0, 5);

        var x = uiBackpack.transform.GetChild(noLongerHovered).GetComponent<Image>().color;
        x.a -= 0.25f;
        uiBackpack.transform.GetChild(noLongerHovered).GetComponent<Image>().color = x;

        var y = uiBackpack.transform.GetChild(hovered).GetComponent<Image>().color;
        y.a += 0.25f;
        uiBackpack.transform.GetChild(hovered).GetComponent<Image>().color = y;
    }


    public Item getHoveredItem()
    {
        if (hovered < eq.Count)
        {
            return eq[hovered];
        }
        return null;
    }

}
