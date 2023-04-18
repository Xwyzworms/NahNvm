using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ghost : Enemy
{
     
    /******************************************************************
        Properties Start

        ghostActiveTimer(4) : Float ==> Ghost able to be seen by player
        ghostActiveCooldown : Float ==> How long the ghost able seen by the player 
        isAggressive : bool ==> Ghost is Aggressive it mean that it will go to player current position
        sr : SpriteRendered ==> This access for making the ghost not visible in the game
        player : Transform ==> Making sure that ghost know the player position

    *******************************************************************/

    private float ghostActiveTimer = 4;
    [SerializeField] private float ghostActiveCooldown;

    private bool isAggresive = true;
    private SpriteRenderer sr;
    
    
    
    /******************************************************************
        Properties End
    *******************************************************************/
    
    
    protected override void Start()
    {
        base.Start();
        facingDirection = facingDirection * -1;
        sr = GetComponent<SpriteRenderer>();

    }

    public void Dissapear() 
    {
        sr.enabled = false;
    }

    public void Appear() 
    {
        sr.enabled = true;
    }

    void Update()
    {

        if(player == null) 
        {
            return;
        }

        ghostActiveTimer -= Time.deltaTime;
        animIdleTimer -= Time.deltaTime;       

        if(ghostActiveTimer >0 ) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        /***************************************************************/
            /*
            If dibawah ini fungsinya untuk adjust aggressive or not aggressive si ghost
            If ghost aggressive maka dia akan execute kode if kedua 
            Otherwise dia bakalan idle, i.e Dissapear
            
            
            ketika GhostActiveTimernya ga aktif dan animIdle timer juga tidak aktif
            DAN DIA itu sedang state aggressive
            play animation untuk dissapear, Jadi dia akan hilang untuk beberapa waktu
            setelah itu baru eksekusi if ke dua, 

            kalau both dibawah 0 dan dia gaAggressive, 
                maka buat dia aggressive dan mendekati player ! dengan set CooldownActiveGhostTimer

            
            */
        /***************************************************************/

        if(ghostActiveTimer < 0 && animIdleTimer < 0 && isAggresive) 
        {
            anim.SetTrigger("ghostDissapear");
            isAggresive = false;
            animIdleTimer = animIdleCooldown;
        }
       
       if(ghostActiveTimer < 0 && animIdleTimer < 0 && !isAggresive ) 
        {
            anim.SetTrigger("ghostAppear");
            isAggresive = true;
            ChoosePosition();
            ghostActiveTimer = ghostActiveCooldown;
        }

        /***************************************************************/

        /***************************************************************/

        if(facingDirection == -1 && transform.position.x < player.transform.position.x) 
        {
            Flip();
        }
        else if(facingDirection == 1 && transform.position.x > player.transform.position.x) 
        {
            Flip();
        }
    }

    public override void Damage()
    {
        if(sr.enabled) 
        {
            base.Damage();
        }
    }

    private void ChoosePosition() 
    {
        float yOffset = Random.Range(-7,7);
        float xOffset = Random.Range(3,5);
        transform.position = new Vector2(player.transform.position.x + xOffset,
                                         player.transform.position.y + yOffset);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if(isAggresive) 
        {
            base.OnTriggerEnter2D(collider);
        }
    }

}
