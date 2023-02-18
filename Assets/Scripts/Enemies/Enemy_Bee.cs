using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bee : Enemy 
{

    /******************************************************************
        This Class having behaviour of EnemyBat & EnemyPlant
    *******************************************************************/
    [Header("Bee behaviour")]
    [SerializeField] private Transform[] idlePoints;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float yOffset;
    [SerializeField] private int destroyBulletTimer = 3;
    private int idlePointIndx = 0;
    private bool attackOver = false;

    [Header("Bullet behaviour")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletPosition;
    [SerializeField] float bulletSpeed;

    private bool isPlayerDetected = false;
    private Transform player;
    private float defaultSpeed;

    private bool isAggresive = false;

    protected override void Start()
    {
        base.Start();
        facingDirection = facingDirection * -1;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        animIdleTimer -= Time.deltaTime;
        anim.SetBool("idle", animIdleTimer > 0);
        if(animIdleTimer > 0) 
        {

            return ;
        }        

        isPlayerDetected = Physics2D.OverlapCircle(transform.position , checkRadius, whatIsPlayer);
        if(isPlayerDetected){
            /// Shoot the player ?
            isAggresive = true;
        }

        if(!isAggresive) 
        {
            transform.position = Vector2.MoveTowards(transform.position, idlePoints[idlePointIndx].position, speed * Time.deltaTime);
            if(Vector2.Distance(transform.position, idlePoints[idlePointIndx].position) < .01f)
            {
                idlePointIndx ++;
                if(idlePointIndx >= idlePoints.Length)
                {
                    idlePointIndx =0;
                }  
            }
            
        }
        else 
        {
                Vector2 newPosition = new Vector2(player.transform.position.x, player.transform.position.y + yOffset);
                transform.position = Vector2.MoveTowards(transform.position, newPosition,speed * Time.deltaTime);
                float xDifference = Mathf.Abs(transform.position.x - newPosition.x);
                if(xDifference < 0.15f) 
                {
                    anim.SetTrigger("enemyBeeAttack");
                }
        }

    }
    private void AttackEvent() 
    {
        GameObject newBullet = Instantiate(bulletPrefab,bulletPosition.position, bulletPosition.rotation);
        newBullet.GetComponent<Enemy_Bullet>().SetupSpeed(0, -bulletSpeed);
        Destroy(newBullet, destroyBulletTimer);
        animIdleTimer = animIdleCooldown;
        isAggresive = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
