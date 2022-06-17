using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactory : StateFactory
{
    EnemyConfig config;
    EnemyStateMachine currentContext;
    
    public EnemyStateFactory(EnemyConfig config, EnemyStateMachine currentContext)
    : base(currentContext) {
        this.config = config;
        this.currentContext = currentContext;
    }

    public State Idle() {
        return new EnemyIdleState(config, currentContext, this);
    }

    public State Aggressive() {
        return new EnemyAggressiveState(config, currentContext, this);
    }

    public State Hurt() {
        return new EnemyHurtState(config, currentContext, this);
    }

}
