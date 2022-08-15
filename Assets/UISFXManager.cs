using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UISFX
{
    enterMenu,
    hoverOver,
    selectOption,
    selectSlider,
    releaseSlider,
    escapeSubMenu,
    escapeMenu,
    exitGame
}

public class UISFXManager : MonoBehaviour
{
    private List<FMOD.Studio.EventInstance> sfxList = new List<FMOD.Studio.EventInstance>();

    private static UISFXManager _instance;
    public static UISFXManager Instance //Singleton Stuff
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Enter Menu"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Hover Over"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Select"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Select"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Release Slider"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Exit Submenu"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Return to Game"));
        sfxList.Add(FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Return to Game"));

    }


    public void PlayMenuSFX(UISFX toPlay)
    {
        sfxList[(int)toPlay].start();

    }

}
