using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    public float projectilespeed = 4.0f;
    public float deacceleration = 0.1f;
    public float knockbackmultiplier = 1.0f;
    public float minimumSpeed = 3.0f;

    private Rigidbody2D rigidBody;

    private float speed;
    private Vector3 velocity;
    public Vector3 Velocity {get {return rigidBody.velocity;} set {rigidBody.velocity = value;}}

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(speed != 0)
        {
            SlowDown(deacceleration);
        }
    }

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
}
