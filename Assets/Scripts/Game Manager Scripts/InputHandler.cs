using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler {

    public static bool BOOL = true;

    // Returns vectorized version of particular axis
    private static Vector3 GetAxisAsVector(string h, string v) {
        float x = Input.GetAxisRaw(h);
        float y = Input.GetAxisRaw(v);
        return new Vector3(x,y,0);
    }

    // Returns which way the player wants to move
    public static Vector3 GetMoveDirection() {
        Vector3 direction = GetAxisAsVector("Move Horizontal", "Move Vertical");
        direction.Normalize();
        BOOL = false;
        return direction;
    }

    // Returns which way the player wants to aim
    public static Vector3 GetAimDirection(Transform transform) {
        Vector3 direction;

        if (Input.GetJoystickNames().Length == 0) {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;
            direction = mouse - transform.position;
        } else {
            float x = Input.GetAxisRaw("Aim Horizontal");
            float y = Input.GetAxisRaw("Aim Vertical");
            direction = new Vector3(x,y,0);
        }
        direction.Normalize();
        return direction;
    }

    // Gets the direction where we should shoot the tongue
    // CURRENTLY FOR EVERY SINGLE THING
    public static Vector3 GetTongueDirection() {
        PlayerConfig player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
        Vector3 direction = GetAimDirection(player.transform);
        if (direction == Vector3.zero) {
            direction = GetMoveDirection();
        }
        if (direction == Vector3.zero) {
            direction = player.directionMap[player.currentdir];
        }
        return direction;
    }

    /* ==============
    ==   BUTTONS   ==
    ============== */

    public static bool Tongue() {
        return Input.GetButtonDown("Tongue");
    }

    public static bool TongueRelease() {
        return !Input.GetButton("Tongue");
    }

    public static bool Staff() {
        return Input.GetButtonDown("Staff");
    }

    public static bool JumpCharge() {
        if (Input.GetJoystickNames().Length > 0) return Input.GetAxis("Jump") > 0.5;
        return Input.GetButtonDown("Jump");
    }

    public static bool JumpRelease() {
        if (Input.GetJoystickNames().Length > 0) return Input.GetAxis("Jump") < 0.5;
        return !Input.GetButton("Jump");
    }

    public static bool Kick() {
        return Input.GetButtonDown("Kick");
    }
}
