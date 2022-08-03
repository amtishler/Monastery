using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HealthTreeFX
{
    getHit,
}

public class HealthTreeSFX : MonoBehaviour
{

    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();



    private void Awake()
    {
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/HealthTree/GetHit"));
    }


    public void PlaySFX(HealthTreeFX toPlay)
    {
        sfxList[(int)toPlay].start();
    }


}
