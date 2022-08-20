using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMusic : MonoBehaviour
{


    private FMOD.Studio.EventInstance cutsceneMusic;

    public string musicEvent;

    public bool fadeOut = false;
    float masterVol = 1;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        cutsceneMusic = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        cutsceneMusic.setParameterByName("MasterVol", 1);
        cutsceneMusic.start();
    }


    private void FixedUpdate()
    {
        if (fadeOut)
        {
            masterVol -= (.25f * Time.deltaTime);
            cutsceneMusic.setParameterByName("MasterVol", masterVol);
        }
    }





}
