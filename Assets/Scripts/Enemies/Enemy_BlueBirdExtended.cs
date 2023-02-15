using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BlueBirdExtended : Enemy_BlueBird
{
    // Start is called before the first frame update
    
    [SerializeField]  Transform[] waypoints;
    [SerializeField] float xMultiplier;
    [SerializeField] float yMultiplier;
    private int waypointIndx = 0;
    private bool goingForward = true;
    private bool flipBack = false;
    protected override void Start()
    {

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

    public override void FlyUpEvent() 
    {
        if(canFly)
        {
            Vector2 distance = this.transform.position - waypoints[waypointIndx].position;
            Debug.Log(distance.x);
            Debug.Log(distance.y);
            if( Vector2.Distance(transform.position,waypoints[waypointIndx].position) < 2.5f) 
            {

                if(goingForward) 
                {
                    waypointIndx ++;
                }
                else {
                    waypointIndx --;
                }


                if(flipBack && waypointIndx == 1) 
                {
                    Flip();
                    flipBack = false;
                }

                if(waypointIndx >= waypoints.Length) 
                {
                    waypointIndx = waypoints.Length - 1;
                    goingForward =false;
                    Flip();
                }
                else if(waypointIndx == 0) 
                {
                    goingForward = true;
                    flipBack = true;
                }
            

                
            }
            rb.velocity = new Vector2(-1 * distance.x * xMultiplier, -1* distance.y *yMultiplier);
        }
    }

}
