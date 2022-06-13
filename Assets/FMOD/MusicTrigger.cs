using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicTrigger
{
    public bool stopMusic;
    public bool triggersCombat;

    public Area area;
    public int variant;

    public bool useDefaultFadeRates;
    public float trackFadeRate;

    public bool[] things;
}
