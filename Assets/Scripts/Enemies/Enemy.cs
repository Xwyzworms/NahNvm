using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    protected int facingDirection = 1; 
    
    
    /******************************************************************
        Collisions Stuffs START
    *******************************************************************/
        [SerializeField] protected LayerMask whatisGround;
        [SerializeField] protected float wallCheckDistance;
        [SerializeField] protected float groundCheckDistance;
        protected bool isWallDetected = false;
        protected bool isGround = false;

        [SerializeField] protected float animIdleTimer; 
        [SerializeField] protected float animIdleCooldown = 2;

    /******************************************************************
        Collisions Stuffs End;
    *******************************************************************/
    

    protected virtual void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
    }

    protected virtual void CollisionCheck()
    {
       isGround = Physics2D.Raycast(groundCheck.transform.position, Vector2.down,  groundCheckDistance, whatisGround);
       isWallDetected = Physics2D.Raycast(wallCheck.transform.position, Vector2.right * facingDirection, wallCheckDistance, whatisGround);

    }

    public void Damage() 
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) 
    {
    
        if(collision.collider.GetComponent<Player>() != null) 
        {
            Player player = collision.collider.GetComponent<Player>();
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
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y) );
    }
}
