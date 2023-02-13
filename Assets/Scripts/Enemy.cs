using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage() 
    {
        Debug.Log("Got Damage");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
    
        if(collision.collider.GetComponent<Player>() != null) 
        {
            Player player = collision.collider.GetComponent<Player>();
            if(player.transform.position.x > this.transform.position.x) 
            {
                player.Knockback(1);
            }
            else if(player.transform.position.x < this.transform.position.x) 
            {
                player.Knockback(-1);
            }
        }
    }
}
