using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public EnemyStateFactory states;
    EnemyConfig config;
    void Start()
    {
        config = GetComponentInParent<EnemyConfig>();
        states = new EnemyStateFactory(config, this);
        currentState = states.Idle();
        currentState.EnterState();
    }

    public override void ForceHurt()
    {
        previousState = currentState;
        currentState.SwitchStates(states.Hurt());
    }

    public override void ForceDead()
    {
        currentState.SwitchStates(states.Dead());
    }

    public override void ForceStunned()
    {
        currentState.SwitchStates(states.Stunned());
    }
}
