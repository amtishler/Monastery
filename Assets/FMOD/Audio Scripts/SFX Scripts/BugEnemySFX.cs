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
    stun
}

public class BugEnemySFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();


    private void Awake()
    {
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BugEnemy/Idle"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BugEnemy/Walk"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BugEnemy/Attack"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BugEnemy/GetHit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BugEnemy/Die"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Generic/Stun"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Scuttlebug/Aggro"));


    }


    public void PlaySFX(BugSFX toPlay)
    {
        sfxList[(int)toPlay].start();
    }
}
