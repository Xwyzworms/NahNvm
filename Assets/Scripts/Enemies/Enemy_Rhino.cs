using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rhino : Enemy
{
    // Start is called before the first frame update
    private bool isAggressive = false;
    [SerializeField] private float agroSpeed;

    private float defaultSpeed;

    [Header("Hit Wall information")]
    private float shockTimer;
    [SerializeField] private float shockTimerCooldown;


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
        CollisionCheck();
        if (playerDetected.collider != null)
        {

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

                if ((!invicible && shockTimer <= 0) || !isGround)
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


        }
    }

    private void AnimationControllers()
    {
        anim.SetFloat("Xvelocity", rb.velocity.x);
        anim.SetBool("isInvicible", invicible);
    }

}
