using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : CharacterConfig {

    public float detectionradius = 10f;
    public float collisiondamage = 0f;
    public float collisionknockback = 10f;
    public float projectileslowdown = 0.1f;
    public GameObject target;

    protected override void _Start() {
        target = FindObjectOfType<PlayerConfig>().gameObject;
        return;
    }

    protected override void _Update() {
        return;
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
    }


    public void RotateSprite(Vector3 targetDir) {
        int angle = GetAngle(targetDir);
        switch(angle){
            case 0:
                spriteRenderer.sprite = moveSpriteList[0];
                break;
            case 2:
                spriteRenderer.sprite = moveSpriteList[1];
                break;
            default:
                break;
        }
        currentdir = angle;
    }

}
