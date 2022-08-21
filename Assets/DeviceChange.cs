using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeviceChange : MonoBehaviour
{
    private FMOD.Studio.EventInstance navigateSFX;
    private FMOD.Studio.EventInstance submitSFX;

    public bool usingController = false;

    private void Awake()
    {
        navigateSFX = FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Hover Over");
        submitSFX = FMODUnity.RuntimeManager.CreateInstance("event:/TriggeredSFX/UI/Escape Menu/Select");
    }

    public void OnControlsChanged() {
        usingController = !usingController;
        if (usingController) EventSystem.current.firstSelectedGameObject.GetComponent<UnityEngine.UI.Selectable>().Select();
        // else EventSystem.current.SetSelectedGameObject(null);
    }

    //Add sound for controller
    //public void OnNavigate() {
        //navigateSFX.start();
    //}

    //public void OnSubmit() {
        //submitSFX.start();
    //}
}
