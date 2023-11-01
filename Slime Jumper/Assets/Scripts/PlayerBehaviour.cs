using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    [SerializeField]private Transform groundCheck;
    [SerializeField]private LayerMask groundLayer;
    private float screenWidth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;

    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Player jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
        //Change player pos if he went through one side of the screen
        Vector3 newPosition = transform.position;

        if (transform.position.x > screenWidth / 2)
        {
            newPosition.x = -screenWidth / 2;
        }
        else if (transform.position.x < -screenWidth / 2)
        {
            newPosition.x = screenWidth / 2;
        }

        transform.position = newPosition;
    }
}
