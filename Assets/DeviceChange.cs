using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeviceChange : MonoBehaviour
{
    public bool usingController;
    public GameObject lastButtonHighlighted;
    public void OnControlsChanged() {
        usingController = !usingController;
        // Debug.Log(usingController);
        if (usingController && lastButtonHighlighted.gameObject.activeInHierarchy) lastButtonHighlighted.GetComponentInParent<UnityEngine.UI.Button>().Select();
        else EventSystem.current.SetSelectedGameObject(null);
    }
}
