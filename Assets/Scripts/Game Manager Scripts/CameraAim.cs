using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraAim : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private InputHandler input;
    private Gamepad gamepad;
    private Vector3 prevPos;
    [SerializeField] private float influence;
    [SerializeField] private GameObject player;

    public void Start() {
        cam = GetComponent<CinemachineVirtualCamera>();
        player = this.gameObject;
        input = GetComponentInParent<InputHandler>();
        gamepad = Gamepad.current;
    }

    public void Update() {
        prevPos = player.transform.localPosition;
        Debug.Log(prevPos);
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), prevPos, Color.blue);
        if (cam.Priority == 10) {
            if (!input.UsingController()) { 
                cam.transform.localPosition = GetMouseInfluence();
                // prevPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            } else { 
                cam.transform.localPosition = GetStickInfluence();
                // prevPos = gamepad.rightStick.ReadValue();
            }
        }
    }

    private Vector3 GetMouseInfluence() {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 direction = prevPos + mouse;
        direction.Normalize();
        direction *= influence;
        direction.z = -10f;
        // prevPos = direction;
        //Debug.Log(direction);
        return direction;
    }

    private Vector3 GetStickInfluence() {
        Vector3 rightStick = gamepad.rightStick.ReadValue();
        rightStick.x *= 10f;
        rightStick.y *= 10f;
        Vector3 direction = prevPos + rightStick;
        direction.Normalize();
        direction *= influence;
        direction.z = -10f;
        // prevPos = direction;
        //Debug.Log(direction);
        return direction;
    }
}
