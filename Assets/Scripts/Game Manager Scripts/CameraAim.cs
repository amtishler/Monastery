using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraAim : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private Gamepad gamepad;

    private Vector3 debugVec;
    private bool gamePaused;

    [SerializeField] private float influence;
    [SerializeField] private GameObject player;

    public void Start() {
        cam = GetComponent<CinemachineVirtualCamera>();
        gamepad = Gamepad.current;
    }

    public void Update() {
        //Debug.DrawLine(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), player.transform.position, Color.blue);
        //Debug.DrawLine(gamepad.rightStick.ReadValue(), player.transform.position, Color.yellow);
        // Debug.Log(player.transform.localPosition);
        if(Time.timeScale == 0) gamePaused = true;
        else gamePaused = false;
        if (cam.Priority == 10 && !gamePaused) {
            if (!InputManager.Instance.UsingController()) { 
                cam.transform.localPosition = GetMouseInfluence();
            } else {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, GetStickInfluence(), 0.05f);
            }
        }
    }

    private Vector3 GetMouseInfluence() {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 direction = mouse - player.transform.position;
        // Debug.Log(direction);
        // Debug.DrawLine(player.transform.position, direction, Color.blue);
        direction.Normalize();
        direction *= influence;
        direction.x -= 0.5f;
        direction.z = -10f;
        debugVec = direction;
        // prevPos = direction;
        //Debug.Log(direction);
        return direction;
    }

    //TO DO: Implement stick deadzones/sensitivity
    private Vector3 GetStickInfluence() {
        Vector3 direction = InputManager.Instance.Aim;
        // Debug.Log(direction);
        // direction.Normalize();
        direction *= influence;
        direction.x -= 0.5f;
        direction.z = -10f;
        debugVec = direction;
        // prevPos = direction;
        // Debug.Log(direction);
        return direction;
    }

    //For debug window
    public Vector3 DebugVec() {
        return debugVec;
    }
}