using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FadeSpeed
{
    normal,
    fast,
    instant,
    slow
}

[System.Serializable]
public class MusicTrigger
{
    /**
     * A NOTE:
     * The way this system is set up, for complete consistency, we will have to specify the on / off information for EACH TRACK in the area's song. Not specifying it will
     * cause it to continue playing
     */

    public static Dictionary<FadeSpeed, float> fadeSpeedDict = new Dictionary<FadeSpeed, float>()
    {
        {FadeSpeed.instant, 10 },
        {FadeSpeed.fast, 2 },
        {FadeSpeed.normal, .4f },
        {FadeSpeed.slow, .2f },
    };

    [System.Serializable]
    public class TrackTriggerInfo
    {
        public bool trackOn;
        public FadeSpeed fadeSpeed;
    }

    // Not all of these fields will have to be assigned in each trigger. Master fade rate won't usually matter unless the area song is just beginning to play or stopping.
    [Header("Master Adjustments")]
    public bool stopMusic;
    public FadeSpeed masterFadeRate;

    [Header("Variant Adjustments")]
    public Area area;
    public int variant;
    public FadeSpeed variantFadeRate;

    //public TrackTriggerInfo[] trackTriggerInfo;
    public MusicTrigger(bool s, FadeSpeed mfr, Area a, int v, FadeSpeed vfr)
    {
        stopMusic = s;
        masterFadeRate = mfr;
        area = a;
        variant = v;
        variantFadeRate = vfr;
    }

}
