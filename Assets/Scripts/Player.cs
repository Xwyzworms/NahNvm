using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{

    //TODO CREATE ANOOOOOOOO, MAke sure the SPRITE IS 16pixel per unity unit
    /**
        player properties

        MECHANICS OF THE PLAYER :

        Able to Do 
        - Jump ( Done )
            - Normal Jump
        - Wall Sliding ( Done )
            - Player able to Wallsliding if touching wall, and when pressed vertical key,it move faster
        - Wall JUmp ( Done)
        - Double Jump ( Done )
        - Buffer Jump ( Done )
            - player able to jump  one more time if they pressing jump button but not touching the ground for some quite amount of time
        - Cayote Jump ()
            - Player able to do jump at the edge of a ground sorting layer objects 

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

    public float jumpForce = 4;
    public Vector2 wallJumpDirection;
    private bool canDoubleJump = false;
    private float defaultJumpForce;
    public float doubleJumpForce ;
    private bool canMove = false;

        /******************************************************************
            BUFFER JUMP Start
        *******************************************************************/
            [Header("Buffer Jump Info")]
            private float bufferJumpTimer = 0;
            [SerializeField]float bufferJumpCooldown = 0.15f;
        /******************************************************************
            BUFFER JUMP End
        *******************************************************************/
    
        /******************************************************************
            Cayote JUMP Start
        *******************************************************************/
            [Header("Cayote Jump Info")]
            private float cayoteJumpTimer = 0;
            [SerializeField]float cayoteJumpCooldown = 0.15f;
            private bool canHaveCayoteJump = false;
        /******************************************************************
            Cayote JUMP End
        *******************************************************************/
    
    /******************************************************************
        JUMP Stuffs END
    *******************************************************************/

    /******************************************************************
       Collision Stuffs START
    *******************************************************************/
    [Header("Collision Detection Info")]
    /*****************************************************************/
    [SerializeField] private LayerMask whatIsGround; // Untuk ignore colliders when Beam start shooting ( Raycast Yak) jadi ini nanti di isi untuk Layer yang akan di raycast
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    bool isMoving = false;
    private bool isGrounded = false;
    private bool isWallDetected = false;
    private bool isWallSliding = false;
    private bool canWallSliding = false;


        /******************************************************************
            Knocked Start
        *******************************************************************/
            [Header("Knocked Info")]
            [SerializeField] private Vector2 knockbackDirection;
            [SerializeField] private bool  isKnocked; 
            [SerializeField] private float knockbackTimer;
            [SerializeField] private float knockbackProtectionTime ;
            [SerializeField] private bool canBeKnocked = true;
        /******************************************************************
            Knocked End
        *******************************************************************/


        /******************************************************************
            Enemy Collision Start
        *******************************************************************/
        [SerializeField] private Transform enemyCheckCollision; //ENemyCheck
        [SerializeField] private float enemyCheckRadius;

        /******************************************************************
            Enemy Collision End
        *******************************************************************/
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
        defaultJumpForce = jumpForce;
    }

    void Update()
    {
        // For visualization of Raycast use the gizmos, DRAW WITH YOURSELF !
        AnimationControllers();
        if(isKnocked)
        {
            return ;
        }

        CollisionCheck();
        FlipController();
        InputChecks();
        checkForEnemyCollision();

        bufferJumpTimer -= Time.deltaTime;
        cayoteJumpTimer -= Time.deltaTime;
        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;
            if(bufferJumpTimer > 0) 
            {
                bufferJumpTimer = -1;
                Jump();
            }
            canHaveCayoteJump = true;
        }
        else 
        {
            // We need boolean to making sure, we are only take one time cayote jump
            if(canHaveCayoteJump) 
            {
                canHaveCayoteJump = false;
                cayoteJumpTimer = cayoteJumpCooldown;
            }
        }
        if(canWallSliding) 
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
            Move();
    }
    private void checkForEnemyCollision() // CheckForEnemy 
    {

        Collider2D[] hittedColliders = Physics2D.OverlapCircleAll(enemyCheckCollision.position, enemyCheckRadius);

        foreach(var enemy in hittedColliders) 
        {
            if(enemy.GetComponent<Enemy>() != null) 
            {
                // Only Damage when Falling, Y < 0
                if(rb.velocity.y < 0)  
                {
                    enemy.GetComponent<Enemy>().Damage();
                    Jump();
                }
            }

        }

    }
    public void Knockback(Transform damagingTransform) 
    {

        if(!canBeKnocked) 
        {
            return;
        }
        #region Define horizontal directionn for knockback
        GetComponent<CameraShakeFX>().screenShake(facingDirection);
        int hDirection = 0;
        isKnocked = true;
        canBeKnocked = false;
        if(this.transform.position.x > damagingTransform.transform.position.x) 
            {
                hDirection = 1;
            }
            else if(this.transform.position.x < damagingTransform.transform.position.x) 
            {
                hDirection = -1;
            }
            else 
            {
                // Knock to Up
                hDirection = 0;
            }
                rb.velocity = new Vector2(knockbackDirection.x * hDirection, knockbackDirection.y);
        #endregion
        Invoke("CancelKnockback", knockbackTimer);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }

    private void CancelKnockback() 
    {
        isKnocked = false;
    }
    private void AllowKnockback() 
    {
        canBeKnocked = true;
    }
    private void AnimationControllers()
    {
        isMoving = rb.velocity.x != 0; // Check if the Player moving or not
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded",isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetBool("isKnocked", isKnocked);

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
        if(!isGrounded) 
        {
            bufferJumpTimer = bufferJumpCooldown;
        }
        if(isWallSliding) 
        {
            WallJump();
            canDoubleJump = true;
        }
       else if (isGrounded || cayoteJumpTimer > 0 )
        {
            Jump();
        }
     
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;
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
        Gizmos.DrawWireSphere(enemyCheckCollision.position, enemyCheckRadius);
    }

}
