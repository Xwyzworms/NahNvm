using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{

    //TODO CREATE ANOOOOOOOO, MAke sure the SPRITE IS 16pixel per unity unit
    /**
        player properties 
*/
    public float moveSpeed;
    public Rigidbody2D rb;

    /**
        JUMP Stuffs START
    */

    public float jumpForce = 2;
    private bool canDoubleJump = false;

    public LayerMask whatIsGround; // Untuk ignore colliders when Beam start shooting ( Raycast Yak) jadi ini nanti di isi untuk Layer yang akan di raycast
    public float groundCheckDistance;
    private bool isGrounded = false;
    /** 
        JUMP Stuffs END
    */
    private float movingInput;

    /**
        player properties 
*/
    void Start()
    {
        Debug.Log("Start Was Called");
    }

    void Update()
    {
        // For visualization of Raycast use the gizmos, DRAW WITH YOURSELF !
        CollisionCheck();

        InputChecks();

        Move();

    }

    void InputChecks()
    {
        movingInput = Input.GetAxisRaw("Horizontal");
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }
    void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }
    }
    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnDrawGizmos()
    {
        /***
            Using the y position because well its for y position tho, lel
        ***/
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance, transform.position.z));
    }

}
