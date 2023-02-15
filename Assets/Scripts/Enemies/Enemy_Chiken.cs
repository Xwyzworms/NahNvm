using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chiken : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        speed= 5;
        facingDirection = facingDirection * -1;
        base.Start();   
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControllers();
        WalkAround();
        
        CollisionCheck();
        
    }

    private void AnimationControllers() 
    {
        anim.SetFloat("Xvelocity", rb.velocity.x);
    }
}
