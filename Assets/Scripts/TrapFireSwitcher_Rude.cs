using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFireSwitcher_Rude : MonoBehaviour
{
    private TrapFire tp;
    private Animator anim; 

    public float x_timeActive = 5;
    public float countdown = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
           
    }
    // Update is called once per frame
    void Update()
    {
        ////YEET
        countdown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        /// If the player already pressed the Button
        // Then YOU CANT Pressed it again for some time !
        if(countdown > 0) 
        {
            return ;
        }

        if(collider.GetComponent<Player>() != null) 
        {
            countdown = x_timeActive;
            anim.SetTrigger("pressed");
            tp.FireSwitchAfter(x_timeActive);
        }


    }
}
