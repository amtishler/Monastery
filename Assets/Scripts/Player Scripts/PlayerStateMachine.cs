using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    PlayerStateFactory states;
    PlayerConfig config;
    void Start()
    {
        config = GetComponentInParent<PlayerConfig>();
        states = new PlayerStateFactory(config, this);
        currentState = states.Idle();
        currentState.EnterState();
    }

    public override void ForceHurt()
    {
        currentState.SwitchStates(states.Hurt());
    }

    public override void ForceDead()
    {
        currentState.SwitchStates(states.Dead());
    }
}