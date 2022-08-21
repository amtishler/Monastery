using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceFader : MonoBehaviour
{
    void Start()
    {
        Debug.Log("starts");

        if (AmbienceManager.Instance != null)
        {
            Debug.Log("should work...");
            AmbienceManager.Instance.StopAmbience();

        }
    }

   
}
