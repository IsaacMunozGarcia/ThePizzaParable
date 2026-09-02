using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    [Header ("Variables")]
     private float horizontal;
     private float vertical;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;
    private float currentJumps;
    private bool canJump;
    
    
    [Header ("References")]
    private Rigidbody2D rigidBody;

    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        currentJumps = jumpCount;
    }
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        ApplyForces();
    }

    private void ApplyForces()
    {
        if (rigidBody.linearVelocity.y == 0)
        {
            rigidBody.AddForce(new Vector2(horizontal * speed, 0));
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.W) && currentJumps > 0 && canJump)
        {
            canJump = false;
            currentJumps--;
            rigidBody.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            currentJumps = jumpCount;
            canJump = true;
        }
    }
}
