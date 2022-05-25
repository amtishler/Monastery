using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : State
{
    public IdleState idleState;
    public AggressiveState aggressiveState;
    public float aggressiveRadius;
    private Collider target;
    public override State RunCurrentState()
    {
        Transform selfTransform = this.transform.parent.gameObject.transform;

        //Target should be set from idle state, otherwise error.
        if(target == null)
        {
            Debug.LogError("No target set");
            return this;
        }
        Transform targetTransform = target.transform;
        float distanceToTarget = Vector3.Distance(selfTransform.position, targetTransform.position);

        RotateToTarget(selfTransform, targetTransform);

        //Return to idle if outside alert range.
        if(distanceToTarget > idleState.detectionRadius)
        {
            target = null;
            return idleState;
        }
        else if(distanceToTarget < aggressiveRadius)
        {
            aggressiveState.setTarget(target);
            return aggressiveState;
        }
        else
        {
            return this;
        }
    }

    private void RotateToTarget(Transform self, Transform target)
    {
        var offset = 90f;
        Vector2 direction = target.position - self.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.parent.gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
    public void setTarget(Collider character)
    {
        target = character;
        return;
    }

    //Added to visualize radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggressiveRadius);
    }
}