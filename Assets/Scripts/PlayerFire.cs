using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject _firePoint, _bulletPrefab;
    [SerializeField] private float startAtkCd;
    [SerializeField] private Backpack bp;
    private float atkCd = 0;

    private void Start()
    {
        bp = GameObject.FindWithTag("Backpack").GetComponent<Backpack>();
        Mathf.Clamp(atkCd, -1, 1000);
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (atkCd <= 0)
            {

                var item = bp.getHoveredItem();
                if (item != null)
                {
                    item.useItem(transform.position);
                    bp.RemoveHoveredFromEq();

                }
                atkCd = startAtkCd;
            }
        }
        atkCd -= Time.deltaTime;
    }
}
