using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneMusic : MonoBehaviour
{
    private FMOD.Studio.EventInstance endMusic;


    // Start is called before the first frame update
    void Start()
    {
        endMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Forest Music");
    }


}
