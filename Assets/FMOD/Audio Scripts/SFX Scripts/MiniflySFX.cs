using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiniflyFX
{
    die,
    fly,
    getHit
}

public class MiniflySFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();


    void Awake()
    {
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Teeny Fly/Die"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Teeny Fly/Fly (Buzz)"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Teeny Fly/Get Hit"));

    }

    public void PlaySFX(MiniflyFX toPlay)
    {
        sfxList[(int)toPlay].start();
    }

}
