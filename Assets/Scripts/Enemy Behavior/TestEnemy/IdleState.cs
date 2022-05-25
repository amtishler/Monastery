using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public AlertState alertState;
    public float detectionRadius;
    public LayerMask detectionLayer;
    public override State RunCurrentState()
    {
        //Test idle state. First scans for characters in the area. Will move to alert if there is a character, otherwise will move idly

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        if (colliders.Length >= 1)
        {
            //Set first thing to cross boundary to be the object alerted
            alertState.setTarget(colliders[0]);
            return alertState;
        }
        else
        {
            return this;
        }
    }



    //Added to visualize radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
