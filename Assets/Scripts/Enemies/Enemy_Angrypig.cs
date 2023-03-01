using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Angrypig : Enemy
{
    
    /******************************************************************
    // State for aggressive pig START
    *******************************************************************/
    private float pigAgressiveTimer;
    [SerializeField] private float pigAgressiveCooldown = 3;
    [SerializeField] private float pigTimerToAgressive = 8;
    [SerializeField] private bool isAgressive = false;

    [SerializeField] private float speedAgressiveMultiplier =  2;

    /******************************************************************
    // State  for Aggressive pig END 
    *******************************************************************/

    /******************************************************************
    // State for Damage pig START
    *******************************************************************/
    [SerializeField] private int hitCounter = 2 ;
    /******************************************************************
    // State  for Damage Pig end
    *******************************************************************/
    
    
    /******************************************************************
    // Start is called before the first frame update
    *******************************************************************/
    protected override void Start()
    {
        base.Start();
        facingDirection = facingDirection * -1;
    }

    // Update is called once per frame
    void Update()
    {
        pigAgressiveTimer -= Time.deltaTime;
        pigTimerToAgressive -= Time.deltaTime;
        CollisionCheck();
        WalkAround();
        AnimationControllers();
        aggresiveController();
    }

    private void aggresiveController() 
    {
        if(pigTimerToAgressive < 0 && !isAgressive) 
        {
            isAgressive = true;
            invicible = false;
            pigAgressiveTimer = pigAgressiveCooldown;
        }

        if(pigAgressiveTimer > 0 && isAgressive) 
        {
            // Change the speed of the monstah
            if(!isWallDetected ) 
            {
                rb.velocity = new Vector2(speed * speedAgressiveMultiplier, rb.velocity.y);
            }
            else 
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("isAgressive", false);
            }
        }
        else if(isAgressive && pigAgressiveTimer < 0)
        {
            isAgressive = false;
            invicible = true;
            pigTimerToAgressive = 8;
        }
    }

    public override void Damage () 
    {
        if(hitCounter > 0 && isAgressive && !invicible) 
        {
            canMove = false;
            hitCounter --;
        }

        if(hitCounter == 0) 
        {
            anim.SetTrigger("hittedByPlayer");
        }
    }

    public override void DestroyGameObject()
    {
            Destroy(gameObject);
    }
    private void AnimationControllers() 
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("isAgressive", isAgressive);
    }

}
