using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraAim : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    [SerializeField] private float mouseInfluence;
    [SerializeField] private GameObject player;

    public void Start() {
        cam = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update() {
        if (cam.Priority == 10) {
            cam.transform.localPosition = GetMouseInfluence();
        }
    }

    private Vector3 GetMouseInfluence() {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouse.z = -10f;
        Vector3 direction = player.transform.localPosition + mouse;
        direction.Normalize();
        direction *= mouseInfluence;
        return direction;
    }
}
