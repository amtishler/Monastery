using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyMinibossSFX
{
    die,
    fly,
    getHit,
    spitProjectile,
    spawnFly,
    stun

}

public class MotherFlySFX : MonoBehaviour
{
    // THE ORDER FOR THIS NEEDS TO MATCH ^ (see awake)
    private static List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();


    // Start is called before the first frame update
    void Awake()
    {
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Die"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Fly (deep buzz)"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Get Hit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Projectile Spit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Spawn Eggs"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Generic/Stun"));

    }

    public void PlaySFX(FlyMinibossSFX toPlay)
    {
        sfxList[(int)toPlay].start();
    }


}
