using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint_Start : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Transform respawnPosition;
    private void Awake()
    {
        PlayerManager.instance.transformPosition= respawnPosition;
    }
    private void Start() 
    {
        anim = this.GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        Player playerComponent = collision.GetComponent<Player>();
        if(playerComponent != null && collision.transform.position.x >  this.transform.position.x) 
        {
            anim.SetTrigger("isTouched");
        }
    }
    
}
