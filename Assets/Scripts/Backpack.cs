using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    [SerializeField] public List<Item> eq = new List<Item>();
    [SerializeField] private GameObject uiBackpack;
    [SerializeField] private Sprite none;
    [SerializeField] private Transform parentOfDrops;
    [SerializeField] private new List<GameObject> children;
    private int hovered = 0;
    public PlayerMovement pm;

    private void Start()
    {
        for (int i = 0; i < eq.Count; i++)
        {
            eq[i] = ScriptableObject.Instantiate(eq[i]);
            eq[i].Init();
        }

        // Debug.Log(GameObject.FindWithTag("Player"));
        // pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        var alpha = uiBackpack.transform.GetChild(0).GetComponent<Image>().color;
        alpha.a += 0.25f;
        uiBackpack.transform.GetChild(hovered).GetComponent<Image>().color = alpha;

        updateHUD();
        pm.UpdateCube(getHoveredItem());
    }

    public bool AddToEq(Item item)
    {
        if (eq.Count < 6)
        {
            eq.Add(item);
            item.value = Random.Range(0, 5);
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
            children[i].GetComponent<EqSlot>().setItem(i < eq.Count ? eq[i] : null, i == hovered);
        }
        pm.UpdateCube(getHoveredItem());
    }

    private void Update()
    {
        if (getHoveredItem() != null)
        {
            pm.isHolding(true);
        }
        else
        {
            pm.isHolding(false);
        }

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

        var it = getHoveredItem();

        updateHUD();
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
