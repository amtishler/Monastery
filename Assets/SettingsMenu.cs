using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsMenu : MonoBehaviour, IPointerEnterHandler
{
float volume;

    Resolution[] resolutions;
    List<string> txt = new List<string>();
    int currentResIndex = 0;
    [SerializeField] private TMP_Text rez;
    [SerializeField] private DeviceChange change;


    bool isFullscreen;

    void Start() {
        if (change == null) change = GameObject.FindGameObjectWithTag("Event").GetComponent<DeviceChange>();
        EventSystem.current.firstSelectedGameObject = GetComponentInChildren<Slider>().gameObject;

        resolutions = Screen.resolutions;
        isFullscreen = Screen.fullScreen;

        //Set volume on start

        for (int i = 0; i < resolutions.Length; ++i) {
            string option = resolutions[i].width + " X " +  resolutions[i].height;
            txt.Add(option);

            if (resolutions[i].width == Screen.width &&
                 resolutions[i].height == Screen.height) {
                    currentResIndex = i;
                    rez.text = option;
                 }
        }
    }

    // Function to change volume
    public void SetVolume(float _volume) {
        volume = _volume;
    }

    public void SetFullscreen(bool _isFullscreen) {
        isFullscreen = _isFullscreen;
    }

    public void ListRight() {
        ++currentResIndex;
        if (currentResIndex < txt.Count) rez.text = txt[currentResIndex];
        else {
            currentResIndex = 0;
            rez.text = txt[currentResIndex];
        }
    }

    public void ListLeft() {
        --currentResIndex;
        if (currentResIndex >= 0) rez.text = txt[currentResIndex];
        else {
            currentResIndex = txt.Count - 1;
            rez.text = txt[currentResIndex];
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        EventSystem.current.SetSelectedGameObject(eventData.pointerEnter);
    }

    public void ApplySettings() {
        Screen.SetResolution(resolutions[currentResIndex].width, resolutions[currentResIndex].height, isFullscreen, 144);

        //Call volume function for changes
        SetVolume(volume);
    }

    public void SetSelected() {
        EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Slider>().gameObject);
    }
}
