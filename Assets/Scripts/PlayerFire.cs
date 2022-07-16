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
        Mathf.Clamp(atkCd, -1, 1000);
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (atkCd <= 0)
            {
                _bulletPrefab = bp.getHoveredItem();
                var endPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
                var diceBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                diceBullet.GetComponent<BulletScript>().setDirection(new Vector2(endPos.x, endPos.y) - (Vector2)transform.position);
                atkCd = startAtkCd;
            }
        }
        atkCd -= Time.deltaTime;
    }
}
