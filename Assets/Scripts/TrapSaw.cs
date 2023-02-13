using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSaw : Trap
{
    // Start is called before the first frame update
    [SerializeField] Animator anim;
    [SerializeField] Transform[] movePoints;
    [SerializeField] float speed = 5;

    [SerializeField] int movePointIndex;
    [SerializeField] float cooldown = 2;
    [SerializeField] float cooldownTimer;

 
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = movePoints[0].position;

    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        bool isWorking = cooldownTimer < 0;
        anim.SetBool("isWorking", isWorking);
        
        if(isWorking) 
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoints[movePointIndex].position, speed* Time.deltaTime);
        }
        if(Vector2.Distance(transform.position, movePoints[movePointIndex].position) < 0.15f) 
        {
            Flip();
            movePointIndex++;
            if(movePointIndex >= movePoints.Length) 
            {
                movePointIndex = 0;
            }
            cooldownTimer = cooldown;
            
        }
    }

    private void Flip() 
    {
        transform.localScale = new Vector3(1,transform.localScale.y * -1);

    }
    
}
