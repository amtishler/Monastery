using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerFX
{
    death,
    eatFruit,
    footsteps,
    getHit,
    kick,
    ribbit,
    staffSwing,
    swallow,
    tongueHit,
    tongueOut
}

public class PlayerSFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();



    private void Awake()
    {
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Death"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Eat Fruit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Footsteps"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/GetHit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Kick"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Ribbit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/StaffSwing"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Swallow"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Tongue Hit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Tongue Out"));
    }


    public void PlaySFX(PlayerFX toPlay)
    {
        sfxList[(int)toPlay].start();
    }


}
