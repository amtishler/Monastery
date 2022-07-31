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
    
    [Header("Weapons")]
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] float attackDeaccel = 1.0f;
    [SerializeField] float kickChargeTime = 0.5f;
    [SerializeField] public GameObject tongue;
    [SerializeField] public GameObject staff;
    [SerializeField] public GameObject kick;

    // Private fields
    private Camera mainCamera;
    private float currentAttackCooldown;

    // Getters & Setters
    public float AttackCooldown { get { return attackCooldown; } }
    public float AttackDeaccel { get { return attackDeaccel; } }
    public float CurrentAttackCooldown { get { return currentAttackCooldown; } }
    public float TongueMaxSpeed { get { return tongueMaxSpeed; } }
    public float KickChargeTime { get { return kickChargeTime; } }
    public bool IsTouchingWall { get; protected set; }

    
    public PlayerAnimator playerAnimator;
    
    public Vector3 resetPosition;

    // Start method, called before the first frame update.
    protected override void _Start()
    {   
        mainCamera = Camera.main;
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    // Update method - just to handle cooldowns.
    protected override void _Update()
    {
        currentAttackCooldown = currentAttackCooldown + Time.deltaTime;
        IsTouchingWall = false;
    }


    // Changes player's sprite to one of the four directions.
    public void RotateSprite(Vector3 targetDir)
    {
        int angle = GetAngle(targetDir);
        // spriteRenderer.sprite = moveSpriteList[angle];
        currentdir = angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("here");
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("hit wall");
            IsTouchingWall = true;
        }
    }
}
