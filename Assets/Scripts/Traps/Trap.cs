using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trap : MonoBehaviour {

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        // Alternative using TAG
        // Tapi kamu juga bisa pake yang namanya 
        //Constancet
        if(collision.GetComponent<Player>() != null) 
        {
            Player player = collision.GetComponent<Player>();
            // If player disebelah kanan, knockback ke kanan,
            // else  kalau disebelah kiri
                player.Knockback(this.transform);
        }
    }   
}