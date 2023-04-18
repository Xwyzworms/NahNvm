using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : Enemy
{

    /*******

        I WROTE TOO MANY COMMENTS, 
        SO ENJOY IT

    *******/

    
    /******************************************************************
        PROPERTIES START@!

        Enemy Bat properties : 

        1. idlePoints : Transform[] --> For BAT gameobject having idle state
        2. isAggressive : bool -> Determine If BAT can fly towards PLAYER or not
        3. canAggressive : bool -> Determine the BAT if CAN ONLY Aggressive if it already wait at IDLE POINT
        4. isPlayerDetected : bool --> DETERMINE wheteher the player inside of THE RADIUS of the bat
        5. Player : Transform --> For location of the player
        6. CheckRadius : Float --> Radius Of the Raycast!
        7. WhatisPlayer : LayerMask --> What layer for bat to detect ! 
        8. Destination : Vector2 --> Used for idlePoints and player position temporay later

        
    *******************************************************************/

    
    [SerializeField] Transform[] idlePoints;
    private bool isAggressive = false;
    private bool canAggressive = true;
    private bool isPlayerDetected = false;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;

    private Vector2 destination;
    [SerializeField] private float defaultSpeed ;

    /******************************************************************
        PROPERTIES END 
    *******************************************************************/

    protected override void Start()
    {
        
        base.Start();
        speed = defaultSpeed;
        destination = idlePoints[0].position;
        this.transform.position = idlePoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)  return;
        AnimationControllers();   
        animIdleTimer -= Time.deltaTime;
        if(animIdleTimer > 0) 
        {
            return ;
        }
        isPlayerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
    /******************************************************************
        This if, is to check if the bat can APPROACH the Player
        if so then the bat can aggressive meaning bat can go toward the player
        because *canAggressive* property set to true, mean the bat currently in IDLE STATE
    *******************************************************************/
        if(isPlayerDetected &&  !isAggressive && canAggressive) 
        {
            isAggressive = true;
            canAggressive = false;
            if(player != null)
                destination = player.transform.position;
            else 
            {
                isAggressive = false;
                canAggressive = true;
            }
        }

    /******************************************************************
    *******************************************************************/
    
    
    /******************************************************************
        THIS if statment is to check whether the bat should go INTO
        PLAYER position or the idle position.
    *******************************************************************/
        if(isAggressive) 
        {
            /******************************************************************
                 If the bat is Aggressive, 
                 then the bat should move toward the player destination with his current speed
                 and alsoo if the bat Close enough to the player
                 TURN OFF *isAggressive* to false
                 and change the destination randomly from the *idlePoints* 
            *******************************************************************/
            transform.position= Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime );
            manageFlip();
            if(Vector2.Distance(transform.position,destination) < 0.5f) 
            {
                isAggressive = false;
                destination = idlePoints[Random.Range(0,idlePoints.Length)].position;
                speed = speed * 0.5f ;
            }
        }
        else 
        {

            /******************************************************************
                If the Bat is NOT aggressive
                then the bat should move towardd the idle position ! with lower speed
                and ALSO Check *canAggressive*, it must be false, since the BAT already close into idle 
                position, But we don't want the BAT go again and scare player,therefore
                we add COOLDOWN ! THAT cooldown is responsible for bat idling before it can DETECT the player again
            *******************************************************************/
            transform.position= Vector2.MoveTowards(transform.position, destination, speed *  Time.deltaTime );
            manageFlip();
            if(Vector2.Distance(transform.position, destination) < 0.3f) 
            {
                    if(!canAggressive) 
                    {
                        animIdleTimer = animIdleCooldown;
                    }
                    canAggressive = true;
                    speed = defaultSpeed;
            }

        }
    /******************************************************************
    *******************************************************************/
    }
    public override void Damage()
    {
        base.Damage();
        animIdleTimer = 4;   
    }
    private void manageFlip() 
    {
        if(this.transform.position.x > destination.x && facingDirection < 0)
        {
            Flip();
        } 
        else if(this.transform.position.x < destination.x && facingDirection > 0)
        {
            Flip();
        }
    }
    private void AnimationControllers() 
    {
        anim.SetBool("canAggressive", canAggressive);
        anim.SetFloat("flyingSpeed", speed);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(this.transform.position, checkRadius);
    }
}
