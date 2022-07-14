using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AreaAmbience
{
    none,
    forest1,

}

public class AmbienceManager : MonoBehaviour
{
    private List<FMOD.Studio.EventInstance> ambienceList = new List<FMOD.Studio.EventInstance>();

    private AreaAmbience currentAreaAmbience;
    private AreaAmbience oldAreaAmbience;

    private FMOD.Studio.EventInstance currentEvent;
    private FMOD.Studio.EventInstance oldEvent;

    private float currentVolume;
    private float oldVolume;

    private bool transitioning;

    [SerializeField]
    private float fadeOutRate;
    [SerializeField]
    private float fadeInRate;


    private static AmbienceManager _instance;
    public static AmbienceManager Instance //Singleton Stuff
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        ambienceList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/AreaAmbiences/Forest"));

        currentVolume = 0;
        oldVolume = 0;
    }

    private void Update()
    {
        //if (currentAreaAmbience != AreaAmbience.none)
            //Debug.Log(currentEvent.getParameterByName("MasterVol"), out );

        if (transitioning)
        {
            bool continueFading = false;

            if (oldVolume > 0 && oldAreaAmbience != AreaAmbience.none)
            {
                oldVolume -= fadeOutRate * Time.deltaTime;
                oldEvent.setParameterByName("MasterVol", oldVolume);
                continueFading = true;
            }

            if (currentVolume < 1 && currentAreaAmbience != AreaAmbience.none)
            {
                currentVolume += fadeInRate * Time.deltaTime;
                currentEvent.setParameterByName("MasterVol", currentVolume);
                continueFading = true;
            }

            transitioning = continueFading;



        }
    }


    public void HandleTrigger(AreaAmbience areaAmbience)
    {
        Debug.Log("trigger");

        if (areaAmbience == currentAreaAmbience)
        {
            return;
        }

        else if (areaAmbience == AreaAmbience.none)
        {
            transitioning = true;

            oldAreaAmbience = currentAreaAmbience;
            oldEvent = currentEvent;
            oldVolume = currentVolume;

            currentAreaAmbience = AreaAmbience.none;
            currentEvent = new FMOD.Studio.EventInstance();
            currentVolume = 0;
        }

        else
        {
            transitioning = true;

            oldAreaAmbience = currentAreaAmbience;
            oldEvent = currentEvent;
            oldVolume = currentVolume;

            oldEvent.setParameterByName("MasterVol", oldVolume);

            currentAreaAmbience = areaAmbience;
            currentEvent = ambienceList[(int)areaAmbience - 1];
            currentVolume = 0;

            currentEvent.setParameterByName("MasterVol", currentVolume);

            currentEvent.start();

        }





    }



}
