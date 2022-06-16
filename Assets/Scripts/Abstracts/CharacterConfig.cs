using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterConfig : MonoBehaviour
{
    // Base variables
    [System.NonSerialized]
    protected CircleCollider2D characterCollider;
    protected SpriteRenderer spriteRenderer;
    protected PlayerStateMachine stateManager;

    [SerializeField] protected float speed = 0f;
    protected Vector3 velocity = Vector3.zero;

    [Header("Movement")]
    [SerializeField] protected float maximumSpeed = 6.0f;
    [SerializeField] protected float minimumSpeed = 3.0f;
    [SerializeField] protected float acceleration = 0.1f;
    [SerializeField] protected float deacceleration = 0.1f;

    [Header("Damage")]
    [SerializeField] protected int hurtiframes = 60;
    [SerializeField] protected float knockbackmultiplier = 60;

    // Serialized Fields
    [Header("Sprites")]
    [SerializeField] protected Sprite[] moveSpriteList = new Sprite[4];

    //For any additional start elements.
    protected abstract void _Start();

    public Vector3 Velocity {get {return velocity;} set {velocity = value;}}
    public float Speed {get {return speed;} set {speed = value;}}
    public float MaximumSpeed {get {return maximumSpeed;}}
    public float MinimumSpeed {get {return minimumSpeed;}}
    public float Acceleration {get {return acceleration;}}
    public float Deacceleration {get {return deacceleration;}}

    void Start()
    {
        characterCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateManager = GetComponent<PlayerStateMachine>();
        _Start();
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

}
