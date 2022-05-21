using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public AlertState alertState;
    public bool inAlertRange;
    public override State RunCurrentState()
    {
        if(inAlertRange)
        {
            return alertState;
        }
        else
        {
            return this;
        }
    }
}
