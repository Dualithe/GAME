using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerObject = null;
    private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;

    void Start()
    {
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

        rb.velocity = (movement * playerSpeed);

    }
}
