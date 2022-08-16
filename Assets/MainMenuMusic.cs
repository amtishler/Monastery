using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{


    private FMOD.Studio.EventInstance mainMenuMusic;

    public bool fadeOut = false;
    float masterVol = 1;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        mainMenuMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Main Menu Music");
        mainMenuMusic.setParameterByName("MasterVol", 1);
        mainMenuMusic.start();
    }


    private void FixedUpdate()
    {
        if (fadeOut)
        {
            masterVol -= (.25f * Time.deltaTime);
            mainMenuMusic.setParameterByName("MasterVol", masterVol);
        }
    }




}
