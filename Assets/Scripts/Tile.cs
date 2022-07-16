using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool isVacant;

    public void Init(bool isOffset)
    {
        isVacant = true;
        _baseColor.a = 1;
        _offsetColor.a = 1;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    public void setVacant(bool vac)
    {
        isVacant = vac;
    }
    public bool getVacant()
    {
        return isVacant;
    }
}
