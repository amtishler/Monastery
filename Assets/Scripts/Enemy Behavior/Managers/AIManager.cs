using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public bool playerDetected;
    public LayerMask detectionLayer;

    [Header("Vision Settings")]
    public float detectionRadius;
    void Awake()
    {
        
    }

    void Update()
    {
        
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            return;
        }

    }
}
