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
        if (cam.Priority == 10) {
            if (!InputManager.Instance.UsingController()) { 
                cam.transform.localPosition = GetMouseInfluence();
            } else {
                cam.transform.localPosition = GetStickInfluence();
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
        Vector2 rightStick = gamepad.rightStick.ReadValue();
        Vector2 playerVec = new Vector2(player.transform.localPosition.x, player.transform.localPosition.y);
        Vector3 direction = rightStick - playerVec;
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