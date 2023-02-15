using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BlueBird : Enemy
{
    protected RaycastHit2D ceillingCheck;
    [SerializeField] protected float ceillingDetectedDistance = 0;
    [SerializeField] protected float flyUpForce;
    [SerializeField] protected float flyDownForce;
    [SerializeField] protected float flyForce;
    protected bool canFly = true;
    protected override void Start()
    {
        facingDirection = facingDirection * -1;
        flyForce = flyUpForce;
        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        CollisionCheck();

        if(ceillingCheck) 
        {
            flyForce = flyDownForce;
        }
        else if(isGround)
        {
            flyForce = flyUpForce;
        }

        if(isWallDetected) 
        {
            Flip();
        }
    }
    public override void Damage()
    {
        base.Damage();
        canFly = false;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0,0);
    }
    public virtual void FlyUpEvent() 
    {
        if(canFly) 
            rb.velocity = new Vector2(speed * facingDirection,  flyForce );
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        ceillingCheck = Physics2D.Raycast(this.transform.position, Vector2.up, ceillingDetectedDistance, whatisGround);

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(this.transform.position, new Vector2(transform.position.x, transform.position.y + ceillingDetectedDistance));
        Gizmos.DrawLine(this.transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }

}
