using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerConfig : CharacterConfig {

    // Base variables
    private CircleCollider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private PlayerStateMachine stateManager;
    private Camera mainCamera;
    [SerializeField] private float speed = 0f;
    private Vector3 velocity = Vector3.zero;

    // Serialized Fields
    [Header("Sprites")]
    [SerializeField] private Sprite[] moveSpriteList = new Sprite[4];
    [Header("Player Movement")]
    [SerializeField] float maximumSpeed = 6.0f;
    [SerializeField] float minimumSpeed = 3.0f;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float deacceleration = 0.1f;
    [SerializeField] float tongueMaxSpeed = 6.0f;
    [Header("Jump Mechanics")]
    [SerializeField] float jumpMaximumSpeed = 12.0f;
    [SerializeField] float jumpMinimumSpeed = 6.0f;
    [SerializeField] float jumpAcceleration = 0.5f;
    [SerializeField] float jumpDeacceleration = 0.5f;
    [SerializeField] float jumpChargeTime = 1.0f;
    [SerializeField] float jumpTotalDist = 8.0f;
    [Header("Weapons")]
    [SerializeField] public GameObject tongue;
    [SerializeField] public GameObject staff;
    [SerializeField] public GameObject kick;

    // Getters & Setters
    public Vector3 Velocity {get {return velocity;} set {velocity = value;}}
    public float Speed {get {return speed;} set {speed = value;}}
    public float MaximumSpeed {get {return maximumSpeed;}}
    public float MinimumSpeed {get {return minimumSpeed;}}
    public float Acceleration {get {return acceleration;}}
    public float Deacceleration {get {return deacceleration;}}
    public float TongueMaxSpeed {get {return tongueMaxSpeed;}}
    public float JumpMaximumSpeed {get {return jumpMaximumSpeed;}}
    public float JumpMinimumSpeed {get {return jumpMinimumSpeed;}}
    public float JumpAcceleration {get {return jumpAcceleration;}}
    public float JumpDeacceleration {get {return jumpDeacceleration;}}
    public float JumpChargeTime {get {return jumpChargeTime;}}
    public float JumpTotalDist {get {return jumpTotalDist;}}



    // Start method, called before the first frame update.
    void Start()
    {   
        playerCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateManager = GetComponent<PlayerStateMachine>();
        mainCamera = Camera.main;
    }


    // Moves according to controls
    public void Move() {

        // finding direction
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 targetDir = new Vector3(x,y,0);
        targetDir.Normalize();

        // moving
        if (targetDir == Vector3.zero) {
            SlowDown(deacceleration);
        } else {
            if (speed < minimumSpeed) speed = minimumSpeed;
            speed = speed + acceleration;
            if (speed > maximumSpeed) speed = maximumSpeed;
            velocity = targetDir*speed;
            Step();
        }

        // update sprite
        if (targetDir == Vector3.zero) return;
        RotateSprite(targetDir);
    }


    // Moves without player input
    public void SlowDown(float dampening) {
        speed = speed - dampening;
        if (speed <= minimumSpeed) speed = 0;
        Vector3 targetDir = velocity;
        targetDir.Normalize();
        velocity = targetDir*speed;
        Step();
    }


    // Steps in direction according to current velocity
    public void Step() {
        transform.Translate(velocity*Time.deltaTime);
    }


    // Gets vector in direction of mouse
    public Vector3 GetMouseDirection() {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        Vector3 direction = mouse - transform.position;
        direction.Normalize();
        return direction;
    }

    // Changes player's sprite to one of the four directions.
    public void RotateSprite(Vector3 targetDir) {
        int angle = GetAngle(targetDir);
        spriteRenderer.sprite = moveSpriteList[angle];
    }

    public int GetAngle(Vector3 targetDir) {
        int angle = (int)Vector3.Angle(targetDir, Vector3.right);
        if (targetDir.y < 0)
            angle = 180 + (int)Vector3.Angle(targetDir, Vector3.left);
        angle = (angle+45)/90;
        if (angle > 3) angle = 0;

        return angle;
    }
}
