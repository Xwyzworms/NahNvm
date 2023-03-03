using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Angrypig : Enemy
{
    
    /******************************************************************
    // State for aggressive pig START
    *******************************************************************/
    [SerializeField]private float pigAgressiveTimer;
    [SerializeField] private float pigAgressiveCooldown = 10;
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
            pigAgressiveTimer = pigAgressiveCooldown;;
        }

        if(pigAgressiveTimer > 0 && isAgressive) 
        {
            // Change the speed of the monstah
            if(!isWallDetected ) 
            {
                rb.velocity = new Vector2(speed * speedAgressiveMultiplier, rb.velocity.y);
            }
            else if(isWallDetected) 
            {
                rb.velocity = Vector2.zero;
                isAgressive = false;
                
                anim.SetBool("isAgressive", isAgressive);
                
                invicible = true;
                pigTimerToAgressive = 8;
            }
        }
    
    }

    public override void Damage () 
    {
        canMove = false;
        rb.velocity =  Vector2.zero;
        if(hitCounter > 0 && isAgressive && !invicible) 
        {
            anim.SetTrigger("hittedByPlayerOnce");
            isAgressive = false;
            hitCounter --;
        }

        if(hitCounter == 0) 
        {
            canMove = false;
            anim.SetTrigger("hittedByPlayer");
        }
        canMove = true;
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
