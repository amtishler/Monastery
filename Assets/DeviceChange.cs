using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeviceChange : MonoBehaviour
{
    public bool usingController = false;

    public void OnControlsChanged() {
        usingController = !usingController;
        if (usingController) EventSystem.current.firstSelectedGameObject.GetComponent<UnityEngine.UI.Selectable>().Select();
        // else EventSystem.current.SetSelectedGameObject(null);
    }

    //Add sound for controller
    // public void OnNavigate() {

    // }

    // public void OnSubmit() {

    // }
}
