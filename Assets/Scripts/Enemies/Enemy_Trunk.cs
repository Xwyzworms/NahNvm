using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trunk : Enemy
{
    // Start is called before the first frame update
    [Header("Trunk properties")]
    [SerializeField] private bool isPlayerDetected = false;
    [SerializeField] private float animShootCooldown = 3;
                     private float animShootTimer ;

    [SerializeField] private Transform wallBehindCheck;
    private bool isWallBehindDetected =false;
    [SerializeField] private float wallBehindDistance;
    [SerializeField] private float wallBehindDetectedCooldown =2;
                     private float wallBehindDetectedTimer = 0;
    private bool isFrontDetected = false;

    [SerializeField] Transform BackPosition;
    [SerializeField] float backRadius;
    [SerializeField] private float backDetectedTimer=0;
    [SerializeField] private float backDetectedCooldown = 2;
    
    private bool isBackDetected = false;
    private float defaultSpeed;
    [SerializeField] LayerMask whatIsPlayer ; 


    [Header("Trunk Bullet properties")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float bulletSpeed;

    protected override void Start()
    {
        base.Start();
        defaultSpeed = speed;
        facingDirection = facingDirection * -1;
    }
    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        isBackDetected = Physics2D.OverlapCircle(BackPosition.position, backRadius, whatIsPlayer);
        isWallBehindDetected = Physics2D.Raycast(wallBehindCheck.position,Vector2.left * facingDirection, wallBehindDistance, whatisGround);
        Debug.Log("Wall Behind + " + isWallBehindDetected);

    }
    // Update is called once per frame
    void Update()
    {
        CollisionCheck();   
        animShootTimer -= Time.deltaTime;
        backDetectedTimer -= Time.deltaTime;
        wallBehindDetectedTimer -= Time.deltaTime;

        if(animShootTimer > 0) 
        {
            rb.velocity = Vector2.zero;
            canMove = false;
        }
        if(isBackDetected )
        {
            if(backDetectedTimer <= 0)
            {
                Flip();
                backDetectedTimer = backDetectedCooldown;
            }
        }
        if(animShootTimer <= 0 && !isPlayerDetected && backDetectedTimer <= 0) 
        {
            canMove = true;
            WalkAround();
        }



        if(playerDetected.collider.GetComponent<Player>() != null  && backDetectedTimer <= 0) 
        {
            // Attack Event
            //this.rb.velocity = Vector2.zero;
            anim.SetTrigger("attackPlayer");
            animShootTimer = animIdleCooldown;
            isPlayerDetected = true;
            if(playerDetected.distance < 4) 
            {
                MoveBackwards(true);
            }

        }
        else {
            
            if(backDetectedTimer > 0 && isBackDetected )
            {
                MoveBackwards(false);
                Flip();
                Debug.Log(rb.velocity.x);
            }
            else if(backDetectedTimer > 0 && !isWallBehindDetected )
            {
                MoveBackwards(true);
                isBackDetected = false;
            }
            isPlayerDetected = false;
        }
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    private void MoveBackwards(bool state)
    {
        if(state)
        {
            rb.velocity = new Vector2(speed * 1.5f * -facingDirection, rb.velocity.y);
        }
        else 
        {
            this.rb.AddForce(new Vector2(speed * 5.5f *facingDirection, rb.velocity.y));
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(BackPosition.position, backRadius);
        Gizmos.DrawLine(wallBehindCheck.position, new Vector2(wallBehindCheck.position.x + wallBehindDistance, wallBehindCheck.position.y));
    }

    public void AttackEvent() 
    {
        Debug.Log("AttacjEbeve");
        GameObject newBullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
        Enemy_Bullet enemyBullet = newBullet.GetComponent<Enemy_Bullet>();
        enemyBullet.SetupSpeed(bulletSpeed * facingDirection, 0);
        Destroy(newBullet, timeDownToDestroy);
    }

}
