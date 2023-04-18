using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeFX : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private Vector3 shakeDirection;
    [SerializeField] private float shakeMultiplier;
    // Start is called before the first frame update
    public void screenShake(int facingDir)
    {
        impulse.m_DefaultVelocity = new Vector3(shakeDirection.x * facingDir, shakeDirection.y) * shakeMultiplier;
        impulse.GenerateImpulse();
    }
    
}
