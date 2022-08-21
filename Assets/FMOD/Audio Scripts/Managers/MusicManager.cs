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
    private List<FMOD.Studio.EventInstance> areaEvents = new List<FMOD.Studio.EventInstance>();

    [SerializeField]
    private string[] variantVolParamNames;
    [SerializeField]
    private string masterVolParamName;

    // Need to store old and current so we can fade in / out songs at the same time
    private FMOD.Studio.EventInstance currentEvent;
    private FMOD.Studio.EventInstance oldEvent;

    private Area currentArea;
    private Area oldArea;

    [SerializeField]
    private int currentVariant;
    [SerializeField]
    private int oldVariant;

    // Master volume here refers to the master volumes of both the ambient and melodic tracks. They should ALWAYS be adjusted together
    private float currentAreaMasterVolume;
    private float oldAreaMasterVolume;

    [SerializeField]
    private float currentVariantVolume;
    [SerializeField]
    private float oldVariantVolume;
    private float currentVariantIncrement;

    private float currentMasterIncrement;

    [SerializeField]
    private bool areaTransitioning;
    [SerializeField]
    private bool variantTransitioning;

    //float counter = 0;

    // For debugger:
    public string[] debugTexts;


    private static MusicManager _instance;
    public static MusicManager Instance //Singleton Stuff
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        areaEvents.Add(FMODUnity.RuntimeManager.CreateInstance("event:/Music/Forest Music"));

    }

    private void Start()
    {
        // TO DO: When implementing a new area, add its ambient and melodic tracks here, in the right order
        currentArea = Area.None;
        areaTransitioning = false;
        variantTransitioning = false;

        currentVariant = -1;
        oldVariant = -1;

        currentVariantVolume = 0;
        oldVariantVolume = 0;

        debugTexts = new string[4] { "", "", "", "" };
            


    }


    private void Update()
    {

        if (areaTransitioning)
        {
            bool continueAreaTransition = false;

            if (currentAreaMasterVolume < 1 && currentArea != Area.None)
            {
                currentAreaMasterVolume += currentMasterIncrement * Time.deltaTime;
                currentEvent.setParameterByName(masterVolParamName, currentAreaMasterVolume);
                continueAreaTransition = true;
            }

            if (oldAreaMasterVolume > 0 && oldArea != Area.None)
            {
                oldAreaMasterVolume -= currentMasterIncrement * Time.deltaTime;
                oldEvent.setParameterByName(masterVolParamName, oldAreaMasterVolume);
                continueAreaTransition = true;
            }

            areaTransitioning = continueAreaTransition;
        }



        if (variantTransitioning)
        {
            bool continueVariantTransition = false;

            if (currentVariantVolume < 1)
            {
                currentVariantVolume += currentVariantIncrement * Time.deltaTime * 6f;
                currentEvent.setParameterByName(variantVolParamNames[currentVariant], currentVariantVolume);
                continueVariantTransition = true;
            }

            if (oldVariantVolume > 0 && oldVariant >= 0)
            {
                //Debug.Log("decrementing");
                oldVariantVolume -= (currentVariantIncrement * Time.deltaTime);
                currentEvent.setParameterByName(variantVolParamNames[oldVariant], oldVariantVolume);
                continueVariantTransition = true;
            }

            variantTransitioning = continueVariantTransition;

        }

        // For debugger:
        debugTexts[0] = currentArea.ToString();
        debugTexts[1] = currentVariant.ToString();
        debugTexts[2] = currentVariantVolume.ToString();
        debugTexts[3] = oldVariantVolume.ToString();
    }

    public void HandleTrigger(bool stopMusic, FadeSpeed masterFadeRate, Area area, int variantIndex, FadeSpeed variantFadeRate)
    {
        MusicTrigger mt = new MusicTrigger(stopMusic, masterFadeRate, area, variantIndex, variantFadeRate);
        HandleTrigger(mt);
    }

    public void HandleTrigger(MusicTrigger trigger)
    {
        if (trigger.stopMusic)
            BeginFadeOut(trigger);

        else if (trigger.area != currentArea)
            BeginFadeToNewArea(trigger);

        else
            BeginFadeToVariant(trigger);

/*        // When combat is triggered:
        oldVariant = MusicManager.Instance.currentVariant;
        HandleTrigger(false, FadeSpeed.normal, Area.Forest, 4, FadeSpeed.normal);

        // When combat finishes:
        HandleTrigger(false, FadeSpeed.normal, Area.Forest, oldVariant, FadeSpeed.normal);*/


    }

    private void BeginFadeOut(MusicTrigger trigger)
    {
        // This function fades out the MASTER VOLUME of an event, leaving the individual tracks alone

        Debug.Log("Fading out");

        areaTransitioning = true;

        oldEvent = currentEvent;
        currentEvent = new FMOD.Studio.EventInstance();

        oldArea = currentArea;
        currentArea = Area.None;

        currentMasterIncrement = MusicTrigger.fadeSpeedDict[trigger.masterFadeRate];

        oldAreaMasterVolume = currentAreaMasterVolume;
        currentAreaMasterVolume = 0;


    }

    private void BeginFadeToNewArea(MusicTrigger trigger)
    {

        Debug.Log("Fading to new area: " + trigger.area.ToString());

        // Will only be executed if areas are different. No need for verification

        // Reassign current variables
        areaTransitioning = true;

        oldEvent = currentEvent;
        currentEvent = areaEvents[((int)trigger.area)];

        oldArea = currentArea;
        currentArea = trigger.area;

        oldVariant = -1;
        currentVariant = trigger.variant;

        currentMasterIncrement = MusicTrigger.fadeSpeedDict[trigger.masterFadeRate];


        // Set master volume to 0, current variant volume to 1
        currentAreaMasterVolume = 0;
        currentEvent.setParameterByName(masterVolParamName, currentAreaMasterVolume);
        foreach (string s in variantVolParamNames)
        {
            currentEvent.setParameterByName(s, 0);
        }
        currentVariantVolume = 1;
        currentEvent.setParameterByName(variantVolParamNames[trigger.variant], currentVariantVolume);
        currentVariant = trigger.variant;


        // Begin event
        currentEvent.start();

    }

    private void BeginFadeToVariant(MusicTrigger trigger)
    {
        Debug.Log("Fading in variant " + trigger.variant + " and fading out variant " + currentVariant);

        // Reassign Variables
        variantTransitioning = true;

        if (oldVariant >= 0)
            currentEvent.setParameterByName(variantVolParamNames[oldVariant], 0f);

        oldVariant = currentVariant;
        oldVariantVolume = currentVariantVolume;
        //Debug.Log("Old: " + oldVariantVolume);

        currentVariant = trigger.variant;
        currentEvent.getParameterByName(variantVolParamNames[currentVariant], out currentVariantVolume);
        //Debug.Log("New: " + currentVariantVolume);

        if (oldVariant == currentVariant)
            oldVariant = -1;

        currentVariantIncrement = MusicTrigger.fadeSpeedDict[trigger.variantFadeRate];

    }

    public int getCurrentVariant()
    {
        return currentVariant;
    }


}