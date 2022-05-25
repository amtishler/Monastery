using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveState : State
{
    public float movementSpeed;
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

        RotateToTarget(selfTransform, targetTransform);
        MoveToTarget(selfTransform, targetTransform);
        return this;
    }
    private void MoveToTarget(Transform self, Transform target)
    {
        self.position = Vector2.MoveTowards(self.position, target.position, movementSpeed * Time.deltaTime);
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

}
