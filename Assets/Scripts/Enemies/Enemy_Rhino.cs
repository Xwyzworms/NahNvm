using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rhino : Enemy
{
    // Start is called before the first frame update
    private bool isAggressive = false;
    private RaycastHit2D playerDetected;
    [SerializeField] private float agroSpeed;

    private float defaultSpeed;

    [Header("Hit Wall information")]
    private float shockTimer;
    [SerializeField] private float shockTimerCooldown;
    [SerializeField] private int distanceToPlayer;


    protected override void Start()
    {
        invicible = true;
        facingDirection = facingDirection * -1;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControllers();
        playerDetected = Physics2D.Raycast(wallCheck.transform.position, Vector2.right * facingDirection, distanceToPlayer, ~whatToIgnore);

        if (playerDetected.collider.GetComponent<Player>() != null)
        {
            isAggressive = true;
        }

        if (isAggressive)
        {
            rb.velocity = new Vector2(agroSpeed * facingDirection, rb.velocity.y);

            if (isWallDetected && invicible)
            {
                invicible = false;
                shockTimer = shockTimerCooldown;
                anim.SetTrigger("IsHittingWall");
            }

            if ((!invicible && shockTimer <= 0 )|| !isGround)
            {
                invicible = true;
                isAggressive = false;
                Flip();
            }
            
            shockTimer -= Time.deltaTime;
        }
        else
        {
            WalkAround();
        }

        CollisionCheck();

    }

    private void AnimationControllers()
    {
        anim.SetFloat("Xvelocity", rb.velocity.x);
        anim.SetBool("isInvicible",invicible);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetected.distance * facingDirection, wallCheck.position.y));
    }
}
