using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFireSwitcher : MonoBehaviour
{
    // Start is called before the first frame update

    public TrapFire tp;
    private Animator anim;
    public int x_seconds = 5;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.GetComponent<Player>() != null) 
        {
            anim.SetTrigger("pressed");       
            tp.FireSwitchAfter(x_seconds);
        }

    }


}
