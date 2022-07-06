using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    // Static doesn't matter so much here, but for enemies it will save some resources
    private static FMOD.Studio.EventInstance footsteps;

    private void Awake()
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
    }


    public void PlayFootSteps()
    {
        footsteps.start();
    }






}
