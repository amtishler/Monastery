using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BirdSFX
{
    idle,
    walk,
    attack,
    getHit,
    die
}

public class BirdEnemySFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();



    private void Awake()
    {
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BirdEnemy/Idle"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BirdEnemy/Walk"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BirdEnemy/Attack"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BirdEnemy/GetHit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/BirdEnemy/Die"));


    }


    public void PlaySFX(BirdSFX toPlay)
    {
        sfxList[(int)toPlay].start();
    }

}
