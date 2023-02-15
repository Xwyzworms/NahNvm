using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Radish : Enemy
{

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

    protected override void Start()
    {
        facingDirection = facingDirection * -1;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
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
