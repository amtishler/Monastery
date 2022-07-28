using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float maxStun = 50f;
    [SerializeField] protected float stun = 0f;
    [SerializeField] protected float stundecay = 0f;
    [SerializeField] protected float invincibleduration = 2f;
    [SerializeField] protected float knockbackmultiplier = 1f;
    [SerializeField] protected float invincibletimer;
    [SerializeField] public float fallDamage = 0f;

    [Header("Sprites")]
    [SerializeField] protected Sprite[] moveSpriteList = new Sprite[4];

    [Header("Falling Values")]
    [SerializeField] public float fallingAnimDuration;
    [SerializeField] public float gravity;
    [SerializeField] public bool grounded;

    [System.NonSerialized] public bool invincible;
    [System.NonSerialized] public bool projectile;
    [System.NonSerialized] public bool grabbable;
    [System.NonSerialized] public bool grabbed;
    [System.NonSerialized] public bool stunned;
    [System.NonSerialized] public bool dead;
    [System.NonSerialized] public int currentdir;
    [System.NonSerialized] public Vector3[] directionMap = new Vector3[4];
    //For any additional elements.
    protected abstract void _Start();
    protected abstract void _Update();

    public Vector3 Velocity {get {return rigidBody.velocity;} set {rigidBody.velocity = value;}}
    public float Speed {get {return speed;} set {speed = value;}}
    public float MaximumSpeed {get {return maximumSpeed;} set {maximumSpeed = value;}}
    public float MinimumSpeed {get {return minimumSpeed;}}
    public float Acceleration {get {return acceleration;}}
    public float Deacceleration {get {return deacceleration;}}
    public float RecoveryDeaccel {get {return recoveryDeaccel;}}
    public float MaxHealth {get {return maxHealth;}}
    public float Health {get {return health;}}
    public float Stun {get {return stun;} set {stun = value;}}
    public float InvincibleTimer {get {return invincibletimer;} set {invincibletimer = value;}}
    public string State {get {return stateManager.currentState.name;}} // Handy for checking which state we're in

    void Start()
    {
        characterCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateManager = GetComponent<StateMachine>();
        rigidBody = GetComponent<Rigidbody2D>();
        directionMap[0] = Vector3.right;
        directionMap[1] = Vector3.up;
        directionMap[2] = Vector3.left;
        directionMap[3] = Vector3.down;
        health = maxHealth;
        grounded = true;
        _Start();
    }
    
    void Update()
    {
        if(invincible && !dead && !grabbed)
        {
            InvincibleTimer += Time.deltaTime;
            if(InvincibleTimer >= invincibleduration)
            {
                invincible = false;
            }
        }

        //TODO stun decay
        /*while(stun > 0){
            stun -= stundecay * Time.deltaTime;
            if(stun < 0){
                stun = 0;
            }
        }*/

        _Update();
    }

    // Stops weapon. Called from an animation event.
    public void StopWeapon(string name)
    {
        transform.Find(name).GetComponent<Attack>().Done = true;
    }

    // Moves without player input
    public void SlowDown(float dampening)
    {
        speed = speed - dampening;
        if (speed <= minimumSpeed) speed = 0;
        Vector3 targetDir = Velocity;
        targetDir.Normalize();
        Velocity = targetDir*speed;
    }

    public void Hit(float damage, float stundamage, Vector3 knockback, float magnitude)
    {
        if(!invincible)
        {
            ApplyKnockback(knockback, magnitude);
            health -= damage;
            stun += stundamage;

            if(health <= 0)
            {
                health = 0;
                stateManager.ForceDead();
                Death(this.gameObject);
                invincible = true;
                dead = true;
            }
            else{
                stateManager.ForceHurt();
                invincible = true;
                InvincibleTimer = 0;
            }

            if(stun >= maxStun)
            {
                stun = maxStun;
                stunned = true;
                stateManager.ForceStunned();
            }
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

    public void Heal(float healamt)
    {
        health += healamt;
        if (health >= maxHealth) health = maxHealth;
    }

    public void Death(GameObject character)
    {
        if (character.CompareTag("Player")) Debug.Log("Death");
        else stateManager.ForceDead();
    }    
}
