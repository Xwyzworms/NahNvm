using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Radish : Enemy
{

    /******************************************************************
        PROPERTIES START
    
    
        groundAboveCheck : bool ==>
        groundBelowCheck : RaycastHit2D ==>
        verticalDirection(1) : Int ==> To told the Radish go up or down

        groundAboveCheckDistance : Float ==> A Distance to check The above Ceilling  
        groundBelowCheckDistance : Float ==> A Distance to check the ground    
        flyForce : float ==> Basically for making the fly slower or not ( int y Axis)
               
        isAggressive(False) : bool ==> Only Aggressive when radish not flying
        aggressiveTimer : Float ==> Timer toControl the flying state for radish
        aggressiveTimeCooldown  : Float ==> it is the cooldown for radish to flying again
        // private float constantRaycastBelowCheck = 1.25f;
    *******************************************************************/

    /******************************************************************
        Radish Flying information Start
    *******************************************************************/
    private bool groundAboveCheck;
    private RaycastHit2D groundBelowCheck;

    private int verticalDirection = 1;

    [SerializeField] private float groundAboveCheckDistance ;
    [SerializeField] private float groundBelowCheckDistance ; 
    [SerializeField] private float flyForce;

    /******************************************************************
        Radish Flying Information End;
    *******************************************************************/

    /******************************************************************
        Radish Aggressive information Start
    *******************************************************************/
    // Only Aggressive when radish not flying xD
    [SerializeField] private bool isAggressive = false;
    private float aggresiveTimer ;
    [SerializeField]private float aggresiveTimeCooldown = 2;
    [SerializeField] private float constantRaycastBelowCheck = 1.25f;
    /******************************************************************
        Radish Aggressive information End
    *******************************************************************/

    /******************************************************************
        PROPERTIES END
    *******************************************************************/


    protected override void Start()
    {
        facingDirection = facingDirection * -1;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    /******************************************************************
        This Update for managing the radish behaviour
    *******************************************************************/
        aggresiveTimer -= Time.deltaTime;



        
        if(aggresiveTimer < 0 && !groundAboveCheck) 
        {
            // In state chillin

            rb.gravityScale = 1;
            isAggressive = false;
        }

        if(!isAggressive) 
        {
            if(groundBelowCheck && !groundAboveCheck) 
            {
               
                verticalDirection = 1;
                    
                
            }
            else if(groundAboveCheck && !groundBelowCheck) 
            {

                verticalDirection = -1;
            }

            
            rb.velocity = new Vector2(0,flyForce*verticalDirection);
            
        }
        else 
        {
                    Debug.Log("Distance" + groundBelowCheck.distance);
            if(groundBelowCheck.distance <= 1.25f && groundBelowCheck.distance> 0)
                {
                    Debug.Log("Distance" + groundBelowCheck.distance);
                    WalkAround();
                }

        }

        AnimationControllers();
        CollisionCheck();
    }

    public override void Damage() 
    {
        /******************************************************************
        when he's flying and got hit by player then he will become aggresive, we will make 
        he's down faster and give some cooldown before he can fly again

        ******************************************************************/
        if(!isAggressive) 
        {
            // Make sure push down faster
            aggresiveTimer = aggresiveTimeCooldown;
            rb.gravityScale = 12;
            isAggressive = true;
        }
        else {
            base.Damage();
        }
    }
    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        
        /******************************************************************
        This collision down here is use for the Celling detection and the Ground detection
        *groundBelowCheck* is TOTALLY DIFFERENT, this one is just check if the raycast Touching the ground layer or not
        ******************************************************************/
        groundAboveCheck = Physics2D.Raycast(transform.position, Vector2.up,  groundAboveCheckDistance, whatisGround);
        groundBelowCheck = Physics2D.Raycast(transform.position, Vector2.down, groundBelowCheckDistance, whatisGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + groundAboveCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundBelowCheckDistance));
    }
    private void AnimationControllers() 
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("isAggresive",isAggressive);
    }
}
