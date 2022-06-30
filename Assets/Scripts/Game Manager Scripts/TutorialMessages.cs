using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessages : GameManager
{
    [SerializeField] public GameObject[] controller;
    [SerializeField] public GameObject[] keyboard;

    private void Start() {
        controller = GameObject.FindGameObjectsWithTag("Gamepad");
        keyboard = GameObject.FindGameObjectsWithTag("Keyboard");
        ShowKeyboard();
    }

    public void ShowKeyboard() {
        foreach (GameObject c in controller) {
            c.SetActive(false);
        }
        foreach (GameObject k in keyboard) {
            k.SetActive(true);
        }
    }

    public void ShowController() {
        foreach (GameObject k in keyboard) {
            k.SetActive(false);
        }
        foreach (GameObject c in controller) {
            c.SetActive(true);
        }
    }

    public GameObject[] GamepadMessages() {
        return controller;
    }

    public GameObject[] KeyboardMessages() {
        return keyboard;
    }
}
