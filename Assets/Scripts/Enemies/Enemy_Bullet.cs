using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : Trap
{
    private Rigidbody2D rb;

    private float xSpeed;
    private float ySpeed; 
    private void Start() {
     rb = this.GetComponent<Rigidbody2D>();   
    }

    private void Update() {
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
