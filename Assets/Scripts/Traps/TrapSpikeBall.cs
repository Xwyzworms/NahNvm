using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikeBall : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    // Start is called before the first frame update
    [SerializeField] Vector2 direction;
    void Start()
    {
        rb.velocity = direction;
    }

    
}
