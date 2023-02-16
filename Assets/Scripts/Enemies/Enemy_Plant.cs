using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletPosition;
    [SerializeField] private float bulletSpeed;
    

    protected override void Start()
    {
        facingDirection = facingDirection * -1;
        base.Start();        
    }

    // Update is called once per frame
    void Update()
    {
        CollisionCheck();
        animIdleTimer  -= Time.deltaTime;
        if(playerDetected.collider != null) 
        {
            bool isPlayerDetected = playerDetected.collider.GetComponent<Player>() != null;
            if(isPlayerDetected && animIdleTimer < 0) 
            {
                anim.SetTrigger("attack");
                animIdleTimer = animIdleCooldown;
            }
        }
    }


    private void AttackEvent() 
    {
        if(playerDetected.collider.GetComponent<Player>() != null) 
        {
            GameObject new_bullet = Instantiate(bulletPrefab, bulletPosition.transform.position, bulletPosition.transform.rotation);
            Enemy_Bullet enemy_bullet = new_bullet.GetComponent<Enemy_Bullet>();
            if(enemy_bullet != null) 
            {
                enemy_bullet.SetupSpeed(bulletSpeed * facingDirection,0);

            }
        }

    }
}
