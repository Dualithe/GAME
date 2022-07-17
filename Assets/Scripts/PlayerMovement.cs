using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerObject = null;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float playerSpeed;

    [SerializeField] private Sprite[] dices;
    [SerializeField] private SpriteRenderer dice;
    private Backpack backpack;

    int diceFrame = 0;

    public void UpdateCube(Item it) {
        dice.gameObject.SetActive(it != null);
        UpdateCubeFrame();
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        backpack = GameObject.FindWithTag("Backpack").GetComponent<Backpack>();
        backpack.pm = this;
        rb = GetComponent<Rigidbody2D>();
        if (playerObject == null)
            playerObject = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(PlayDiceAnimation());
        UpdateCube(backpack.getHoveredItem());
    }

    private void UpdateCubeFrame() {
        var it = backpack.getHoveredItem();
        if (it != null) {
            dice.sprite = dices[it.id * 6 + diceFrame];
        }
    }

    IEnumerator PlayDiceAnimation() {
        diceFrame = 0;
        while (true) {
            diceFrame = (diceFrame + 1) % 6;
            UpdateCubeFrame();
            yield return new WaitForSeconds(0.12f); 
        }
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
