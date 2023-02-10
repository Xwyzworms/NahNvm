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

    private Rigidbody2D rb;
    private Animator anim;


    /*****************************************************************/
   [Header("Move info")]
    public float moveSpeed;
    private bool facingRight = true;
    private int facingDirection = 1;
    /*****************************************************************/
    
    /******************************************************************
        JUMP Stuffs START
    *******************************************************************/

    public float jumpForce = 2;
    public Vector2 wallJumpDirection;
    private bool canDoubleJump = false;

    private bool canMove = false;

    /******************************************************************
        JUMP Stuffs END
    *******************************************************************/

    /******************************************************************
       Collision Stuffs START
    *******************************************************************/
    [Header("Collision Detection Info")]
    /*****************************************************************/
    public LayerMask whatIsGround; // Untuk ignore colliders when Beam start shooting ( Raycast Yak) jadi ini nanti di isi untuk Layer yang akan di raycast
    public float groundCheckDistance;
    public float wallCheckDistance;
    bool isMoving = false;
    private bool isGrounded = false;
    private bool isWallDetected = false;
    private bool isWallSliding = false;
    private bool canWallSliding = false;

    /***************************************************************/
    /******************************************************************
        Collision Stuffs END
    *******************************************************************/

    private float movingInput;

    /**
        player properties 
*/
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // For visualization of Raycast use the gizmos, DRAW WITH YOURSELF !
        AnimationControllers();
        CollisionCheck();
        FlipController();
        InputChecks();

        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;
        }
        if(canWallSliding) 
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }

            Move();

    }
    private void AnimationControllers()
    {
        isMoving = rb.velocity.x != 0; // Check if the Player moving or not
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded",isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);

    }
    private void InputChecks()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        // If player pressed the  S, then can WallSliding False,
        // Make the movement of sliding faster if pressed, slower when not
        if(Input.GetAxis("Vertical") < 0) 
        {
            canWallSliding = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }
    private void FlipController() 
    {
        if(facingRight && rb.velocity.x < -.1f) 
        {
            Flip();
        }
       
        else if(!facingRight && rb.velocity.x > .1f)
        {   
            Flip();
        }
    }
    private void Flip() 
    {
        facingDirection = facingDirection * -1; // For The Direction represented by int, For gizmos , detecting wall
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }
    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position,  Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        if(isWallDetected && rb.velocity.y < 0) 
        {
            canWallSliding = true;
        }
        if(!isWallDetected) 
        {
            isWallSliding = false;
            canWallSliding = false;
        }
    }
    private void JumpButton()
    {
        if(isWallSliding) 
        {
            WallJump();
        }
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }

        canWallSliding = false;
    }
    private void Move()
    {
        if(canMove) 
        {
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
        }
    }

    private void WallJump() 
    {
            canMove = false;
            rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection,  wallJumpDirection.y);
    }

    private void Jump()
    {
        Debug.Log(jumpForce + " " + rb.velocity.y.ToString());
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        Debug.Log(jumpForce + " " + rb.velocity.y.ToString());
    }

    private void OnDrawGizmos()
    {
        /***
            Using the y position because well its for y position tho, lel
        ***/
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallCheckDistance * facingDirection , transform.position.y));
    }

        
}
