using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerObject = null;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float playerSpeed;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        GameObject.FindWithTag("Backpack").GetComponent<Backpack>().pm = this;
        rb = GetComponent<Rigidbody2D>();
        if (playerObject == null)
            playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);


    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(x, y);
        if (x < 0)
        {
            playerObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (x == 0) { }
        else
        {
            playerObject.GetComponent<SpriteRenderer>().flipX = false;
        }


        rb.velocity = (movement * playerSpeed);
        if (Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.y) > 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);

        }
    }

    public void isHolding(bool ih)
    {
        animator.SetBool("IsHolding", ih);
    }
}
