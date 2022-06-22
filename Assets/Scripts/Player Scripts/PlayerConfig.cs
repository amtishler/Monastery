using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class PlayerConfig : CharacterConfig {

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
    [SerializeField] float tongueCooldown = 1.0f;
    [SerializeField] public GameObject staff;
    [SerializeField] float staffCooldown = 1.0f;
    [SerializeField] public GameObject kick;
    [SerializeField] float kickCooldown = 1.0f;

    // Private fields
    private Camera mainCamera;
    private InputHandler input;
    private float currentTongueCooldown;
    private float currentStaffCooldown;
    private float currentKickCooldown;

    // Getters & Setters
    public InputHandler Input {get {return input;}}
    public float TongueCooldown {get {return tongueCooldown;}}
    public float StaffCooldown {get {return staffCooldown;}}
    public float KickCooldown {get {return kickCooldown;}}
    public float CurrentTongueCooldown {get {return currentTongueCooldown;} set {currentTongueCooldown = value;}}
    public float CurrentStaffCooldown {get {return currentStaffCooldown;} set {currentStaffCooldown = value;}}
    public float CurrentKickCooldown {get {return currentKickCooldown;} set {currentKickCooldown = value;}}
    public float TongueMaxSpeed {get {return tongueMaxSpeed;}}
    public float JumpMaximumSpeed {get {return jumpMaximumSpeed;}}
    public float JumpMinimumSpeed {get {return jumpMinimumSpeed;}}
    public float JumpAcceleration {get {return jumpAcceleration;}}
    public float JumpDeacceleration {get {return jumpDeacceleration;}}
    public float JumpChargeTime {get {return jumpChargeTime;}}
    public float JumpTotalDist {get {return jumpTotalDist;}}
    
    public PlayerAnimator playerAnimator;

    // Start method, called before the first frame update.
    protected override void _Start() {   
        mainCamera = Camera.main;
        input = GetComponentInParent<InputHandler>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    // Update method - just to handle cooldowns.
    protected override void _Update() {
        currentTongueCooldown = currentTongueCooldown + Time.deltaTime;
        currentStaffCooldown = currentStaffCooldown + Time.deltaTime;
        currentKickCooldown = currentKickCooldown + Time.deltaTime;
    }


    // Changes player's sprite to one of the four directions.
    public void RotateSprite(Vector3 targetDir) {
        int angle = GetAngle(targetDir);
        //spriteRenderer.sprite = moveSpriteList[angle];
        currentdir = angle;
    }
}
