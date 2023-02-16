using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
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

    // Update is called once per frame
    private void Update() {
    }

    protected virtual void CollisionCheck()
    {
        if(groundCheck != null) 
        {

            isGround = Physics2D.Raycast(groundCheck.transform.position, Vector2.down,  groundCheckDistance, whatisGround);
        }
        if(wallCheck != null) 
        {
            isWallDetected = Physics2D.Raycast(wallCheck.transform.position, Vector2.right * facingDirection, wallCheckDistance, whatisGround);
        }
        playerDetected = Physics2D.Raycast(wallCheck.transform.position, Vector2.right * facingDirection, distanceToPlayer, ~whatToIgnore);

    }
    protected virtual void WalkAround() 
    {
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
