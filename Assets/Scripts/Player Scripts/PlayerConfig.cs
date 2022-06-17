using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerConfig : CharacterConfig {


    private Camera mainCamera;

    // Serialized Fields
    [Header("Tongue")]
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
    public float TongueMaxSpeed {get {return tongueMaxSpeed;}}
    public float JumpMaximumSpeed {get {return jumpMaximumSpeed;}}
    public float JumpMinimumSpeed {get {return jumpMinimumSpeed;}}
    public float JumpAcceleration {get {return jumpAcceleration;}}
    public float JumpDeacceleration {get {return jumpDeacceleration;}}
    public float JumpChargeTime {get {return jumpChargeTime;}}
    public float JumpTotalDist {get {return jumpTotalDist;}}

    public int currentdir;

    // Start method, called before the first frame update.
    protected override void _Start()
    {   
        mainCamera = Camera.main;
    }


    // // Moves according to controls
    // public void Move() {

    //     // finding direction
    //     float x = Input.GetAxisRaw("Horizontal");
    //     float y = Input.GetAxisRaw("Vertical");
    //     Vector3 targetDir = new Vector3(x,y,0);
    //     targetDir.Normalize();

    //     // Moving
    //     if (targetDir == Vector3.zero) {
    //         SlowDown(deacceleration);
    //     } else {
    //         if (speed < minimumSpeed) speed = minimumSpeed;
    //         speed = speed + acceleration;
    //         if (speed > maximumSpeed) speed = maximumSpeed;
    //         velocity = targetDir*speed;
    //         ResetVelocity();
    //     }

    //     // Update sprite
    //     if (targetDir == Vector3.zero) return;
    //     RotateSprite(targetDir);
    // }


    // Changes player's sprite to one of the four directions.
    public void RotateSprite(Vector3 targetDir) {
        int angle = GetAngle(targetDir);
        spriteRenderer.sprite = moveSpriteList[angle];
        currentdir = angle;
    }


    // Gets vector in direction of mouse
    public Vector3 GetMouseDirection() {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        Vector3 direction = mouse - transform.position;
        direction.Normalize();
        return direction;
    }
}
