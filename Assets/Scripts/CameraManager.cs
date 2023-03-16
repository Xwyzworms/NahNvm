using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject myCamera;
    [SerializeField] private Color gizmosColor;
    [SerializeField] private PolygonCollider2D cd;
    private void OnStart() 
    {
        myCamera.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
    }
    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.GetComponent<Player>() != null) 
        {
            myCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if(collider.GetComponent<Player>() != null) 
        {
            myCamera.SetActive(false);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(cd.bounds.center, cd.bounds.size);
    }
}
