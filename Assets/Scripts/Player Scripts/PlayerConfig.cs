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
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] float attackDeaccel = 1.0f;
    [SerializeField] public GameObject tongue;
    [SerializeField] public GameObject staff;
    [SerializeField] public GameObject kick;

    // Private fields
    private Camera mainCamera;
    private float currentAttackCooldown;

    // Getters & Setters
    public float AttackCooldown {get {return attackCooldown;}}
    public float AttackDeaccel {get {return attackDeaccel;}}
    public float CurrentAttackCooldown {get {return currentAttackCooldown;}}
    public float TongueMaxSpeed {get {return tongueMaxSpeed;}}
    public float JumpMaximumSpeed {get {return jumpMaximumSpeed;}}
    public float JumpMinimumSpeed {get {return jumpMinimumSpeed;}}
    public float JumpAcceleration {get {return jumpAcceleration;}}
    public float JumpDeacceleration {get {return jumpDeacceleration;}}
    public float JumpChargeTime {get {return jumpChargeTime;}}
    public float JumpTotalDist {get {return jumpTotalDist;}}
    
    public PlayerAnimator playerAnimator;
    
    public Vector3 resetPosition;

    // Start method, called before the first frame update.
    protected override void _Start() {   
        mainCamera = Camera.main;
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    // Update method - just to handle cooldowns.
    protected override void _Update() {
        currentAttackCooldown = currentAttackCooldown + Time.deltaTime;
    }


    // Changes player's sprite to one of the four directions.
    public void RotateSprite(Vector3 targetDir) {
        int angle = GetAngle(targetDir);
        // spriteRenderer.sprite = moveSpriteList[angle];
        currentdir = angle;
    }
}
