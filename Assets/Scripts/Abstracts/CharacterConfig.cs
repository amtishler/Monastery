using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterConfig : MonoBehaviour
{
    // Base variables
    [System.NonSerialized]
    protected CircleCollider2D characterCollider;

    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected StateMachine stateManager;

    [SerializeField] protected float speed = 0f;

    [Header("Movement")]
    [SerializeField] protected float maximumSpeed = 6.0f;
    [SerializeField] protected float minimumSpeed = 3.0f;
    [SerializeField] protected float acceleration = 0.1f;
    [SerializeField] protected float deacceleration = 0.1f;
    [SerializeField] protected float recoveryDeaccel = 0.5f;

    [Header("Damage")]
    [SerializeField] protected float invincibletimer = 1.5f;
    [SerializeField] protected float knockbackmultiplier = 1f;

    [Header("Sprites")]
    [SerializeField] protected Sprite[] moveSpriteList = new Sprite[4];

    public bool invincible;
    public int currentdir;
    //For any additional start elements.
    protected abstract void _Start();

    public Vector3 Velocity {get {return rigidBody.velocity;} set {rigidBody.velocity = value;}}
    public float Speed {get {return speed;} set {speed = value;}}
    public float MaximumSpeed {get {return maximumSpeed;}}
    public float MinimumSpeed {get {return minimumSpeed;}}
    public float Acceleration {get {return acceleration;}}
    public float Deacceleration {get {return deacceleration;}}
    public float RecoveryDeaccel {get {return recoveryDeaccel;}}

    void Start() {
        characterCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateManager = GetComponent<StateMachine>();
        rigidBody = GetComponent<Rigidbody2D>();
        _Start();
    }

    // Moves without player input
    public void SlowDown(float dampening) {
        speed = speed - dampening;
        if (speed <= minimumSpeed) speed = 0;
        Vector3 targetDir = Velocity;
        targetDir.Normalize();
        Velocity = targetDir*speed;
    }

    public void Hit(float damage, Vector3 knockback, float magnitude)
    {
        if(!invincible)
        {
            stateManager.ForceHurt();
            ApplyKnockback(knockback, magnitude);
        }
    }

    public void ApplyKnockback(Vector3 dir, float mag)
    {
        speed = mag * knockbackmultiplier;
        Velocity = dir;
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
