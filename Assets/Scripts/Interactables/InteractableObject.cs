using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    public float deacceleration = 0.5f;
    public float knockbackmultiplier = 1.0f;
    public float minimumSpeed = 3.0f;

    protected Rigidbody2D rigidBody;

    protected float speed;
    protected Vector3 velocity;
    public Vector3 Velocity {get {return rigidBody.velocity;} set {rigidBody.velocity = value;}}

    public void SlowDown(float dampening) {
        speed = speed - dampening;
        if (speed <= minimumSpeed) speed = 0;
        Vector3 targetDir = Velocity;
        targetDir.Normalize();
        Velocity = targetDir*speed;
    }

    public void ApplyKnockback(Vector3 dir, float mag)
    {
        speed = mag * knockbackmultiplier;
        Velocity = dir;
    }

    public virtual void OnHit(Vector3 dir, float mag){}
}