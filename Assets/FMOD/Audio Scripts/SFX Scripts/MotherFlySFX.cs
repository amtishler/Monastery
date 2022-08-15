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
    private List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();

    private static PlayerConfig pConfig;

    public MusicCollider mc;

    // Start is called before the first frame update
    void Awake()
    {
        pConfig = GameObject.FindObjectOfType<PlayerConfig>();

        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Die"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Fly (deep buzz)"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Get Hit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Projectile Spit"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Fly Miniboss/Spawn Eggs"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/Enemies/Generic/Stun"));

    }

    public void PlaySFX(FlyMinibossSFX toPlay)
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

    public void StopCombatMusic()
    {
        mc.gameObject.SetActive(false);

        MusicManager.Instance.HandleTrigger(false, FadeSpeed.normal, Area.Forest, 3, FadeSpeed.normal);
    }

}
