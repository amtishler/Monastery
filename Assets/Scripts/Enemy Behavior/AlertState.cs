using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : State
{
    public IdleState idleState;
    public bool inVisionRange;
    public override State RunCurrentState()
    {
        if(!inVisionRange)
        {
            return idleState;
        }
        else
        {
            return this;
        }
    }
}
