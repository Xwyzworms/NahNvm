using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // All access to the player is managed here, SIngleton

    public static PlayerManager instance;
    [SerializeField] public Transform transformPosition;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] public GameObject currentPlayer;

    private void Awake() 
    {
        PlayerManager INSTANCE = instance;
        if(INSTANCE == null) 
        {
            instance = this;
        }
        PlayerRespawn();       
    }

    public void PlayerRespawn()
    {
        if(currentPlayer  == null) 
        {
            currentPlayer = Instantiate(playerPrefab, transformPosition.position, this.transform.rotation);
        }
    }
}
