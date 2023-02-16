using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ghost : Enemy
{
     
    private float ghostActiveTimer = 4;
    [SerializeField] private float ghostActiveCooldown;

    private bool isAggresive = true;
    private SpriteRenderer sr;
    private Transform player;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        facingDirection = facingDirection * -1;
        player = GameObject.Find("Player").transform;   
        sr = GetComponent<SpriteRenderer>();
        Debug.Log(player.transform.position);

    }

    public void Dissapear() 
    {
        sr.enabled = false;
    }

    public void Appear() 
    {
        sr.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

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
