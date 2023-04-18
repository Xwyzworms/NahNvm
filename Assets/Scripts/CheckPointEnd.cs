using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointEnd : MonoBehaviour
{


    //TODO:
    /*
        1. Create Animation components
        2. Check if player collision with the EndCheckpoint
        3. if collision, activated the trigger
        4. Anim play endlessly
        n. Stop the game tho if hitted
    */
    [SerializeField] private Transform transformLocation;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.GetComponent<Player>() != null) 
        {
            anim.SetTrigger("pressed");
            // STOP the game, or continue
        }       
    }

}
