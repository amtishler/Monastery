using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BugSFX
{
    idle,
    walk,
    attack,
    getHit,
    die,
    stun,
    aggro,
    windup
}

public class BugEnemySFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();

    private static PlayerConfig pConfig;


    private void Awake()
    {
        pConfig = GameObject.FindObjectOfType<PlayerConfig>();

        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/Idle"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/Footsteps"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/Lunge"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/GetHit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/Die"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Generic/Stun"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/Aggro"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/Wind Up"));

    }


    public void PlaySFX(BugSFX toPlay)
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
