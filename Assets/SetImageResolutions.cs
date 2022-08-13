using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageResolutions : MonoBehaviour
{
    private Image[] images;

    private void Awake() {
        images = GetComponentsInChildren<Image>();
        foreach (var i in images) {
            i.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
            i.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        }
    }
}
