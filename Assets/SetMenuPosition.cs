using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMenuPosition : MonoBehaviour
{
    private Image[] images;
    
    //Menu dimensions
    [SerializeField] private float menuX = 0f;
    [SerializeField] private float menuY = 0f;
    [SerializeField] public bool debug = false;

    private void Awake() {
        images = GetComponentsInChildren<Image>();
        foreach (var i in images) {
            i.rectTransform.sizeDelta = new Vector2(menuX, menuY);
            i.rectTransform.position = new Vector2(Screen.width/2, Screen.height/2);
        }
    }
    private void Update() {
        if (debug) {
            foreach (var i in images) {
                i.rectTransform.sizeDelta = new Vector2(menuX, menuY);
            }   
        }
    }
}

