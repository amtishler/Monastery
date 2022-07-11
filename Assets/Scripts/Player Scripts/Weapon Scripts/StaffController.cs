using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StaffController : Attack {
    
    [SerializeField] BoxCollider2D staffCollider;
    [SerializeField] float staffSwingTime;

    private float timeUp = 0f;

    // Disable method, called when staff is disabled.
    void OnDisable() {
        timeUp = 0f;
    }

    // Update is called once per frame
    public void UpdateStaff() {
        timeUp = timeUp + Time.deltaTime;
    }

    // Whether or not staff is done
    public bool Done() {
        return timeUp >= staffSwingTime;
    }
}
