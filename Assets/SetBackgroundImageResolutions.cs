using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBackgroundImageResolutions : MonoBehaviour
{
    private Image[] images;

    private void Awake() {
        images = GetComponentsInChildren<Image>();
        foreach (var i in images) {
            i.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            i.rectTransform.position = new Vector2(Screen.width/2, Screen.height/2);
        }
    }
}
