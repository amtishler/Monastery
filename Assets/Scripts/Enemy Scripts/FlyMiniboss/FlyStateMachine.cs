using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStateMachine : StateMachine
{
    FlyStateFactory states;
    FlyConfig config;
    void Start()
    {
        config = GetComponentInParent<FlyConfig>();
        states = new FlyStateFactory(config, this);
        currentState = states.Idle();
        currentState.EnterState();
    }

    
    public override void ForceHurt()
    {
        //previousState = currentState;
        //currentState.SwitchStates(states.Hurt());
    }

    public override void ForceDead()
    {
        //currentState.SwitchStates(states.Dead());
    }

    public override void ForceStunned()
    {
        //currentState.SwitchStates(states.Stunned());
    }
}
