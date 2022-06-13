using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    /**
     * Important Notes:
     *  -This system can handle at most 2 songs playing at the same time. Currently uncertain what happens if you move through 3 areas, but hopefully that won't come up
     *  -The "oldArea" and "oldEvent" variables designate what should be fading out
     *  -All changes to FMOD parameters happen in the Update method. I think this is simpler and might save coroutine confusion
     *  -
     */

    // VERY IMPORTANT to make sure the order stays consistent so that it matches up with the area enum
    [SerializeField]
    private FMOD.Studio.EventInstance[] FMODEvents;

    [SerializeField]
    private string[] ubiqTrackParamNames;
    [SerializeField]
    private string ubiqMasterVolParam;

    private FMOD.Studio.EventInstance currentEvent;
    private FMOD.Studio.EventInstance oldEvent;
    private Area currentArea;
    private Area oldArea;
    private int currentVariation;
    private LinkedList<bool[]> trackOnOffInfo;

    private float currentMasterVolume;
    private float oldMasterVolume;
    private float[] currentTrackVolumes;
    private float currentTrackIncrement;

    [SerializeField]
    private float defaultTrackIncrement;
    [SerializeField]
    private float defaultMasterIncrement;

    private bool oldMasterFading;
    private bool newMasterFading;


    // Could make ^^ change based on music trigger but it seems kind of unneccesary for now

    private void Start()
    {
        oldMasterFading = false;
        newMasterFading = false;
    }


    private void Update()
    {
        if (oldMasterFading)
        {
            if (oldMasterVolume > 0)
            {
                oldMasterVolume -= (defaultMasterIncrement * Time.deltaTime);
                oldEvent.setParameterByName(ubiqMasterVolParam, oldMasterVolume);
            }

            if (oldMasterVolume < 0)
            {
                oldMasterFading = false;
                ;

            }


        }


        //instance.setParameterByName("Reverb", reverb);
    }

    public void HandleMusicTrigger(MusicTrigger trigger)
    {
        if (trigger.stopMusic)
            BeginFadeOut(trigger);

        else if (trigger.area != currentArea)
            BeginFadeToNewArea(trigger);

        else
            BeginFadeToVariant(trigger);

    }

    private void BeginFadeOut(MusicTrigger trigger)
    {
        // This function fades out the MASTER VOLUME of an event, leaving the individual tracks alone

        oldMasterFading = true;

        oldArea = currentArea;
        currentArea = Area.None;

        oldEvent = currentEvent;
        // Might not work??
        currentEvent = new FMOD.Studio.EventInstance();

        float oldVol;
        oldEvent.getParameterByName(ubiqMasterVolParam, out oldVol);
        oldMasterVolume = oldVol;


    }

    private void BeginFadeToNewArea(MusicTrigger trigger)
    {
        
    }

    private void BeginFadeToVariant(MusicTrigger trigger)
    {

    }






}