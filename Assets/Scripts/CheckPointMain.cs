using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMain : MonoBehaviour
{

    //TODO
    /*
    // 1. Create Checkpoint bool  
    // 2. Create Checkpoint ANimation
    // 3. Change the RespawnPoint of player to checkpoint, when triggerExit Occurs  
    // 4. Setup the Animator
    */
    // Start is called before the first frame update
    [SerializeField] Transform checkpointPosition;
    private Animator anim;
    private bool checkpointStatus = false;


    void Start()
    {
        anim = this.GetComponent<Animator>();
        anim.SetBool("isActivated", checkpointStatus);
    }


    private void OnTriggerExit2D(Collider2D collision) 
    {
        // Do 
        if(checkpointStatus == false && collision.GetComponent<Player>() != null)
        {
            checkpointStatus = true;
            anim.SetBool("isActivated", checkpointStatus);
        }

        if(checkpointStatus) 
        {
            Player playerComponent = collision.GetComponent<Player>();
            if(playerComponent != null && playerComponent.transform.position.x < collision.transform.position.x) 
            {
                changeRespawnlocation();
            }
        
        }
        
    }

    private void changeRespawnlocation() 
    {
        PlayerManager.instance.transformPosition = this.transform;
    }
}
