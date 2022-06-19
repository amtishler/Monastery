using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDebugger : MonoBehaviour
{
    [SerializeField]
    public MusicTrigger[] triggers;


    private void Update()
    {
        if(Input.GetKeyDown("[0]"))
        {
            MusicManager.Instance.HandleMusicTrigger(triggers[0]);
        }

        if (Input.GetKeyDown("[1]"))
        {
            MusicManager.Instance.HandleMusicTrigger(triggers[1]);
        }

        if (Input.GetKeyDown("[2]"))
        {
            MusicManager.Instance.HandleMusicTrigger(triggers[2]);
        }

        if (Input.GetKeyDown("[3]"))
        {
            MusicManager.Instance.HandleMusicTrigger(triggers[3]);
        }

    }


}
