using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject _firePoint, _bulletPrefab;
    [SerializeField] private float startAtkCd;
    [SerializeField] private Backpack bp;
    private Animator animator;
    private float atkCd = 0;

    private void Start()
    {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
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
                    animator.SetBool("IsThrowing", true);

                }
                else
                {
                    animator.SetBool("IsThrowing", false);
                }
                atkCd = startAtkCd;
            }
        }
        else
        {
            animator.SetBool("IsThrowing", false);
        }
        atkCd -= Time.deltaTime;
    }
}
