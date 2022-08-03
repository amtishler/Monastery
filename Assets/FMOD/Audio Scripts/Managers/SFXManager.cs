using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    /**
     * Unlike the Music Manager, which stores instances of music and whatnot, the SFX Manager is
     * only designed to store some global functions and audio information. 
     * 
     * (!) FMOD Event instances for SFX are handled within scripts assigned to the causer of the SFX
     */

    private static SFXManager _instance;
    public static SFXManager Instance //Singleton Stuff
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }


    


    public void AssignSFXVCA(float gain)
    {

    }






}
