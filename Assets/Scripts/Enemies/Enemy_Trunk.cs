using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trunk : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private float checkPlayerRadius;
    [SerializeField] private bool isPlayerDetected = false;
    [SerializeField] private float animShootCooldown = 3;
                     private float animShootTimer ;
                    

    protected override void Start()
    {
        base.Start();
        facingDirection = facingDirection * -1;
    }

    // Update is called once per frame
    void Update()
    {

        if(playerDetected.collider.GetComponent<Player>() != null) 
        {
            // Attack Event
            anim.SetTrigger("attackPlayer");
        }

        WalkAround();
        CollisionCheck();   
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    private void AttackEvent() 
    {
        
    }
}
