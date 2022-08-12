using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BirdSFX
{
    idle,
    walk,
    attack,
    getHit,
    die,
    stun,
    aggro
}

public class BirdEnemySFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();

    private static PlayerConfig pConfig;

    private void Awake()
    {
        pConfig = GameObject.FindObjectOfType<PlayerConfig>();

        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Landbird/Idle"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Landbird/Footsteps"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Landbird/Attack"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Landbird/GetHit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Landbird/Die"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Generic/Stun"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Landbird/Aggro"));



    }


    public void PlaySFX(BirdSFX toPlay)
    {
        float dist = Vector3.Distance(pConfig.transform.position, transform.position);
        float volumeMod = ((18 - dist) / 18) + .1f;

        if (volumeMod <= 0)
        {
            return;
        }

        sfxList[(int)toPlay].setParameterByName("MasterVol", volumeMod);

        sfxList[(int)toPlay].start();
    }

}
