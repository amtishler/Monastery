using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : CharacterConfig {

    public float detectionradius = 10f;
    public float attackradius = 4f;
    public float collisiondamage = 0f;
    public float collisionknockback = 10f;
    public float projectileslowdown = 0.1f;

    //Stalking speed (Getting ready to attack)
    public float readyingspeed = 1f;

    //Time spent stalking (before attacking)
    public float attacktimer = 1f;

    //Pouncing speed;
    public float pouncespeed = 10f;
    public float pounceslowdown = 0.1f;

    public float attackcooldown = 5f;
    public bool oncooldown;
    private bool timerstarted;
    private float cooldowntimer;

    public GameObject target;
    public Vector3 attackvector;

    protected override void _Start() {
        target = FindObjectOfType<PlayerConfig>().gameObject;
        return;
    }

    protected override void _Update() {
        if(!timerstarted && oncooldown)
        {
            cooldowntimer = 0;
            timerstarted = true;
        }
        if(timerstarted)
        {
            cooldowntimer += Time.deltaTime;
            if(cooldowntimer > attackcooldown)
            {
                oncooldown = false;
                timerstarted = false;
            }
        }
    }

    public void MoveTowards(GameObject target)
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        targetDir.Normalize();

        // Moving
        if (targetDir == Vector3.zero) {
            SlowDown(deacceleration);
        } else {
            if (speed < minimumSpeed) speed = minimumSpeed;
            speed = speed + acceleration;
            if (speed > maximumSpeed) speed = maximumSpeed;
            Velocity = targetDir*speed;
        }

        // Update sprite TODO
        RotateSprite(targetDir);
    }

    //Added to visualize radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionradius);
        Gizmos.DrawWireSphere(transform.position, attackradius);
    }


    public void RotateSprite(Vector3 targetDir) {
        int angle = GetAngle(targetDir);
        switch(angle){
            case 0:
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case 2:
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            default:
                break;
        }
        currentdir = angle;
    }

}
