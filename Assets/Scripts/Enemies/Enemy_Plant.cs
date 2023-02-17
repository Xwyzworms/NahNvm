using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{

    /*******

        I WROTE TOO MANY COMMENTS, 
        SO ENJOY IT

    *******/


    /******************************************************************
        PROPERTIES START@!

        Enemy Bat properties : 

        1. bulletPrefab : GameObject --> Prefab to instansiate the bullet later on game
        2. bulletPosition : Transform --> Location where the bullet will out !
        3. bulletSpeed : float --> Speed of the bullet
        
    *******************************************************************/
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletPosition;
    [SerializeField] private float bulletSpeed;

    /******************************************************************
        @ ! PROPERTIES END  ! @
    *******************************************************************/
    
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
        /******************************************************************
            this if is to check if the RAYCAST able to detect a collider
            and if its the PLAYER, then the enemy plant going to attack
            **AttackEvent()** happen when the trigger *attack* executed
            Then the plant cannot be shoot again for *animIdleCooldown*
        *******************************************************************/
        if(playerDetected.collider != null) 
        {
            bool isPlayerDetected = playerDetected.collider.GetComponent<Player>() != null;
            if(isPlayerDetected && animIdleTimer < 0) 
            {
                anim.SetTrigger("attack");
                animIdleTimer = animIdleCooldown;
            }
        }
        /******************************************************************
        *******************************************************************/
    }


    private void AttackEvent() 
    {

        
        /******************************************************************
            This if, is to check if the enemy plant able to detect the player
            if so
            - Create a bullet at *bulletPosition.transform* and *bulletPosition.transform.rotation*
            - Get the **Enemy_Bullet** component, and then
            - Setup its speed, to making sure its move !
        *******************************************************************/
        if(playerDetected.collider.GetComponent<Player>() != null) 
        {
            GameObject new_bullet = Instantiate(bulletPrefab, bulletPosition.transform.position, bulletPosition.transform.rotation);
            Enemy_Bullet enemy_bullet = new_bullet.GetComponent<Enemy_Bullet>();
            if(enemy_bullet != null) 
            {
                enemy_bullet.SetupSpeed(bulletSpeed * facingDirection,0);

            }
        }
        /******************************************************************
        *******************************************************************/

    }
}
