using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour{

    private PlayerConfig config;
    private bool usingController;

    private Vector3 move;
    private Vector3 aim;
    private bool tongue;
    private bool staff;
    private bool kick;
    private bool jump;

    // Getters & setters
    public Vector3 Move {get {return move;}}
    public Vector3 Aim {get {return GetAim();}}
    public bool Tongue {get {if (config.CurrentTongueCooldown > config.TongueCooldown) return tongue; else return false;}}
    public bool Staff {get {if (config.CurrentStaffCooldown > config.StaffCooldown) return staff; else return false;}}
    public bool Kick {get {if (config.CurrentKickCooldown > config.KickCooldown) return kick; else return false;}}
    public bool Jump {get {return jump;}}

    // On Start
    private void Start() {

        if (InputSystem.GetDevice<Gamepad>() != null) usingController = true;
        else usingController = false;

        config = GetComponentInParent<PlayerConfig>();

        move = Vector3.zero;
        aim = Vector3.zero;
        tongue = false;
        staff = false;
        kick = false;
        jump = false;
    }

    // Input listeners
    private void Update() {}

    private void OnControlsChanged() {
        usingController = !usingController;
    }

    private void OnMove(InputValue value) {
        move = value.Get<Vector3>(); 
        move.Normalize();
    }

    private void OnAim(InputValue value) {
        aim = value.Get<Vector3>();
        aim.Normalize();
    }

    private void OnTongue(InputValue value) {
        tongue = value.isPressed;

    }

    private void OnStaff(InputValue value) {
        staff = value.isPressed;
    } 

    private void OnKick(InputValue value) {
        kick = value.isPressed;
    }

    private void OnJump(InputValue value) {
        jump = value.isPressed;
    }

    // Helper functions if needed
    public Vector3 GetMouseDirection() {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouse.z = 0f;
        Vector3 direction = mouse - config.transform.position;
        direction.Normalize();
        return direction;
    }

    public bool TongueHeld() {
        return tongue;   // I need to think of something better
    }

    public Vector3 GetAim() {
        Vector3 direction;
        if (usingController) direction = aim;
        else direction = GetMouseDirection();
        if (direction == Vector3.zero) direction = move;
        if (direction == Vector3.zero) direction = config.directionMap[config.currentdir];
        return direction;
    }
}
