using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqSlot : MonoBehaviour
{
    [SerializeField] private Image dice;
    [SerializeField] private Image dicePlates;
    [SerializeField] private Image dark;
    [SerializeField] private Sprite[] diceImages;
    [SerializeField] private Sprite[] dicePlateImages;
    public void setItem(Item it, bool selected)
    {
        if (it == null)
        {
            dicePlates.gameObject.SetActive(false);
            dice.gameObject.SetActive(false);
        }
        else
        {
            dicePlates.gameObject.SetActive(true);
            dice.gameObject.SetActive(true);
            dicePlates.sprite = dicePlateImages[it.id / 6 + it.value];
            dice.sprite = diceImages[it.id / 6];
        }
        dark.gameObject.SetActive(!selected);
    }

}
