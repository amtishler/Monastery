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
    [SerializeField] private float influence;
    [SerializeField] private GameObject player;

    public void Start() {
        cam = GetComponent<CinemachineVirtualCamera>();
        input = GetComponentInParent<InputHandler>();
        gamepad = Gamepad.current;
    }

    public void Update() {
        //Debug.DrawLine(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), player.transform.position, Color.blue);
        //Debug.DrawLine(gamepad.rightStick.ReadValue(), player.transform.position, Color.yellow);
        // Debug.Log(player.transform.localPosition);
        if (cam.Priority == 10) {
            if (!input.UsingController()) { 
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
        // prevPos = direction;
        //Debug.Log(direction);
        return direction;
    }

    private Vector3 GetStickInfluence() {
        Vector3 rightStick = gamepad.rightStick.ReadValue();
        // rightStick.x *= 10f;
        // rightStick.y *= 10f;
        Vector3 direction = rightStick - player.transform.localPosition;
        // Debug.Log(direction);
        direction.Normalize();
        direction *= influence;
        direction.x -= 0.5f;
        direction.z = -10f;
        // prevPos = direction;
        //Debug.Log(direction);
        return direction;
    }
}