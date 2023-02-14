using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSawExtended : Trap
{

    [SerializeField] Transform[] movePoints; 
    [SerializeField] private float speed = 5;
    private int indexMovePoint;
    [SerializeField] private bool goingForward = true;
    private bool isWorking = false;
    private Animator anim; 
    // Start is called before the first frame update
    void Start()
    {   
        anim = GetComponent<Animator>();
        isWorking = true;
        transform.position = movePoints[0].position;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[indexMovePoint].position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoints[indexMovePoint].position) < 0.15) 
        {
            if (goingForward) 
            {
                indexMovePoint ++;
            }
            else 
            {
                indexMovePoint --;
            }

            if(indexMovePoint >= movePoints.Length ) 
            {
                indexMovePoint = movePoints.Length-1;
                goingForward = false;
            }
            else if(indexMovePoint == 0) 
            {

                goingForward = true;
            }
        }           
    }
}
