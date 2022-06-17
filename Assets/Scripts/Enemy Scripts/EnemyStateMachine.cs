using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    EnemyStateFactory states;
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
        currentState.SwitchStates(states.Hurt());
    }
}
