using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : CharacterConfig {

    public float detectionradius = 10f;
    public GameObject target;

    protected override void _Start()
    {
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
            velocity = targetDir*speed;
            Step();
        }

        // Update sprite TODO
    }

    //Added to visualize radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionradius);
    }

}
