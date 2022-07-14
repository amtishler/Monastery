using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerFX
{
    walkGrass,
    walkDirt,
    walkRock,
    staffSwing,
    kick,
    spit
}

public class PlayerSFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();



    private void Awake()
    {
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Footsteps"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Footsteps"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/Footsteps"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Player/StaffSwing"));


    }


    public void PlaySFX(PlayerFX toPlay)
    {
        sfxList[(int)toPlay].start();
    }


}
