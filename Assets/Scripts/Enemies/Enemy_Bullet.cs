using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : Trap
{

    /*******

        I WROTE TOO MANY COMMENTS, 
        SO ENJOY IT

    *******/


    /******************************************************************
        PROPERTIES START@!

        Enemy Bullet properties : 

        1.rb : Rigidbody2D --> ITS because the bullets need to use VELOCITY 
        2.xSpeed : Float --> Basically the speed of bullets !
        3.ySpeed : Float --> this is just the y axis

    *******************************************************************/
    private Rigidbody2D rb;

    private float xSpeed;
    private float ySpeed;

    /******************************************************************
        PROPERTIES END @!
    *******************************************************************/
    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Destroy(gameObject);
    }

    public void SetupSpeed(float xSpeed, float ySpeed)
    {
        this.xSpeed = xSpeed;
        this.ySpeed = ySpeed;
    }
}
