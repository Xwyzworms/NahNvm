using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    /*******

        I WROTE TOO MANY COMMENTS, 
        SO ENJOY IT

    *******/
    
    /******************************************************************
        PROPERTIES START #1 !

        Enemy Class properties : 

        1. speed : Float ==> just a default speed for each movable enemy
        2. anim : Animator ==> animator controller for each enemy
        3. rb : rb ==>  for physics stuff, so the game object affected by physics
        4. groundCheck : Transform ==> Transform for checking ground, it basically prevent game object to kept stay on their own platformers
        5. wallCheck : Transform ==> Transform for wall checking, so you can flip the enemy if there's wall infront of it
        
        6. playerDetected : RaycastHit2D ==> This just a raycast for checking if the enemy able to see player
        7. facingDirection (1) : int ==>  A value to check if the FacingDirection is right or left
        8. Invicible(false) : bool ==> Basically to determine if the enemy able to DESTROYED or not, 
        9. canMove(true) : bool ==> First state for making sure that the enemy able to move 
        10. whatIsGround : LayerMask ==> Layer mask for checking the enemy placed in ground or not*groundCheck*
        
        11. whatToIgnore : LayerMask ==> Layer mask for ignoring a layer, this is used only for detecting player ;
        12. wallCheckDistance(0.8f) : float ==> The distance for raycast to check 
        13. groundCheckDistance(0.93f) : float ==>   The distance for raycast to check 
        14. isWallDetected(false) : Bool ==> Just a general checking of a wall for flipping gameObject
        15. isGround(false) : Bool ==>  JUst a boolean to check if the enemy currently in ground

        16. animIdleTimer : float ==> Basically untuk cooldown suatu event; 
        17. animIdleCooldown(2) : Float ==> Cooldown untuk *animIdleTimer*;

        18.[SerializeField] protected int distanceToPlayer;
        
    *******************************************************************/
    [SerializeField] protected float speed = 3;
    protected Animator anim;
    protected Rigidbody2D rb;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    protected RaycastHit2D playerDetected;

    protected int facingDirection = 1;
    protected bool invicible = false;

    protected bool canMove = true;
    
    
    /******************************************************************
        Collisions Stuffs START
    *******************************************************************/
        [SerializeField] protected LayerMask whatisGround;
        [SerializeField] protected LayerMask whatToIgnore;
        [SerializeField] protected float wallCheckDistance = 0.88f;
        [SerializeField] protected float groundCheckDistance  = 0.93f; 
        protected bool isWallDetected = false;
        protected bool isGround = false;

        [SerializeField] protected float animIdleTimer; 
        [SerializeField] protected float animIdleCooldown = 2;

        [SerializeField] protected int distanceToPlayer;
    /******************************************************************
        Collisions Stuffs End;
    *******************************************************************/
    

    protected virtual void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();

        if(groundCheck == null)
        {
            groundCheck = transform;
        }
        if(wallCheck == null) 
        {
            wallCheck = transform;
        }
    }

    private void Update() {
        /*******

            IN Here, there's only for the child implementation
            SO ENJOY IT

        *******/
    }

    protected virtual void CollisionCheck()
    {

    /******************************************************************
        Basically this only for collision check that ARE GENERAL 
        For each enemy

        It just checking the transform of *groundCheck* and *wallCheck*
        and if exists then, do raycast ( Shoot a bean),if the beam hit the ground/wall
        then it should be TRUE for both value!

        playerDetected is a different one, It is ignore the defined LayerMask
    *******************************************************************/


        if(groundCheck != null) 
        {

            isGround = Physics2D.Raycast(groundCheck.transform.position, Vector2.down,  groundCheckDistance, whatisGround);
        }
        if(wallCheck != null) 
        {
            isWallDetected = Physics2D.Raycast(wallCheck.transform.position, Vector2.right * facingDirection, wallCheckDistance, whatisGround);
        }
        playerDetected = Physics2D.Raycast(wallCheck.transform.position, Vector2.right * facingDirection, distanceToPlayer, ~whatToIgnore);
    
    /******************************************************************
    
    *******************************************************************/

    }
    protected virtual void WalkAround() 
    {
    /******************************************************************
        //TODO
    *******************************************************************/
        if (animIdleTimer <= 0 && canMove)
        {
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (isWallDetected || !isGround)
        {
            Flip();
            animIdleTimer = animIdleCooldown;
        }
        animIdleTimer -= Time.deltaTime;
    }
    public virtual void Damage() 
    {
        if(!invicible) 
        {
            canMove = false;
            anim.SetTrigger("hittedByPlayer");
        }
    }

    public void DestroyGameObject() 
    {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) 
    {
    
        if(collider.GetComponent<Player>() != null) 
        {
            Player player = collider.GetComponent<Player>();
            player.Knockback(this.transform);
        }
    }

    protected virtual void Flip() 
    {
        /**********
            Flip the gameobject when detecting the wall
        ***/
        facingDirection= facingDirection * -1;
        transform.Rotate(0,180,0); 
    }

    protected virtual void OnDrawGizmos() 
    {
        if(groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        if(wallCheck != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y) );
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetected.distance * facingDirection, wallCheck.position.y));

        }
    }
}
