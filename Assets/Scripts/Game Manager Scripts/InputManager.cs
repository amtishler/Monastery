using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Button
public class Button {

    private bool pressed;
    private bool pressedBuffer;
    private bool held;
    public bool Pressed {get {return pressed;}}
    public bool Held {get {return held;}}

    public Button() {
        pressed = false;
        pressedBuffer = false;
        held = false;
    }
    public void SetValue(bool isDown) {
        held = isDown;
        pressed = isDown;
        pressedBuffer = isDown;
    }
    public void Update() {
        pressed = pressedBuffer;
        pressedBuffer = false;
    }
}

// Input Handler
public class InputManager : MonoBehaviour{

    private static InputManager _instance;
    public static InputManager Instance {
        get {
            if (_instance == null) Debug.Log("Input Manager is Null");
            return _instance;
        }
    }

    private PlayerConfig config;

    private PlayerInput playerInput;
    private bool usingController = false;

    // Used for tutorial messages
    private TutorialMessages messages;
    private bool tutorialActive = false;
    /////////////////////////////////////
    private Vector3 move;
    private Vector3 aim;
    private Button tongue;
    private Button staff;
    private Button kick;
    private Button jump;
    private Button reset;
    private Button interact;
    private Button debugMenu;

    // Getters & Setters
    public Vector3 Move {get {return move;}}
    public Vector3 Aim {get {return GetAim();}}
    public bool TonguePressed {get {return tongue.Pressed;}}
    public bool TongueHeld {get {return tongue.Held;}}
    public bool StaffPressed {get {return staff.Pressed;}}
    public bool StaffHeld {get {return staff.Held;}}
    public bool KickPressed {get {return kick.Pressed;}}
    public bool KickHeld {get {return kick.Held;}}
    public bool JumpPressed {get {return jump.Pressed;}}
    public bool JumpHeld {get {return jump.Held;}}
    public bool ResetPressed {get {return reset.Pressed;}}
    public bool ResetHeld {get {return reset.Held;}}
    public bool InteractPressed {get {return interact.Pressed;}}
    public bool InteractHeld {get {return interact.Held;}}
    public bool DebugMenuPressed {get {return debugMenu.Pressed;}}
    public bool DebugMenuHeld {get {return debugMenu.Held;}}
    
    // For debugger:
    public string inputText = "";


    // On Start
    private void Awake() {
        if (InputSystem.GetDevice<Gamepad>() != null) usingController = true;
        else usingController = false;

        // messages = GameManager.Instance.GetTutorialMessages();
        // tutorialActive = messages.tutorial;
        // Debug.Log(messages.controller[0] + " -- " + messages.keyboard[0]);
        
        config = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
        playerInput = GetComponentInParent<PlayerInput>();

        move = Vector3.zero;
        aim = Vector3.zero;
        tongue = new Button();
        staff = new Button();
        kick = new Button();
        jump = new Button();
        reset = new Button();
        interact = new Button();
        debugMenu = new Button();

        inputText = "";

        _instance = this;
    }

    // Makes sure "pressed" is set to false if somebody just holds the button down.
    // EVERY BUTTON NEEDS TO BE CALLED HERE, since they are NOT monobehaviours and
    // can't have their own Update() method.
    private void Update() {
        tongue.Update();
        staff.Update();
        kick.Update();
        jump.Update();
        interact.Update();
    }

    private void OnControlsChanged() {
        usingController = !usingController;
        if (tutorialActive) ChangeMessages();
    }

    private void OnMove(InputValue value) {
        move = value.Get<Vector3>(); 
        move.Normalize();
        //inputText = "Moving";
    }

    private void OnAim(InputValue value) {
        aim = value.Get<Vector3>();
        aim.Normalize();
        //inputText = "Aiming";
    }

    private void OnTongue(InputValue value) {
        tongue.SetValue(value.isPressed);
        //inputText = "Tonguing";
    }

    private void OnStaff(InputValue value) {
        staff.SetValue(value.isPressed);
        //inputText = "Staffing";
    } 

    private void OnKick(InputValue value) {
        kick.SetValue(value.isPressed);
        //inputText = "Kicking";
    }

    private void OnJump(InputValue value) {
        jump.SetValue(value.isPressed);
        //inputText = "Jumping";
    }

    private void OnReset(InputValue value) {
        reset.SetValue(value.isPressed);
        //inputText = "Reseting";
    }

    private void OnInteract(InputValue value) {
        interact.SetValue(value.isPressed);
    }

    private void OnDebugMenu(InputValue value) {
        debugMenu.SetValue(value.isPressed);
        //inputText = "Turning on debug";
    }

    // Helper functions if needed
    public Vector3 GetMouseDirection() {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouse.z = 0f;
        Vector3 direction = mouse - config.transform.position;
        direction.Normalize();
        return direction;
    }

    public Vector3 GetAim() {
        Vector3 direction;
        if (usingController) direction = aim;
        else direction = GetMouseDirection();
        if (direction == Vector3.zero) direction = move;
        if (direction == Vector3.zero) direction = config.directionMap[config.currentdir];
        return direction;
    }

    public void DeathMap() {
        playerInput.SwitchCurrentActionMap("Death");
    }

    private void ChangeMessages() {
        if (usingController) {
            messages.ShowController();
        } else {
            messages.ShowKeyboard();         
        }
    }

    public bool UsingController() {
        return usingController;
    }
}