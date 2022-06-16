using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactory : StateFactory
{
    EnemyConfig config;
    
    public EnemyStateFactory(EnemyConfig config, EnemyStateMachine currentContext)
    : base(currentContext) {
        this.config = config;
    }

    public State Idle() {
        return new EnemyIdleState(config, context, this);
    }

    public State Aggressive() {
        return new EnemyAggressiveState(config, context, this);
    }

    public State Hurt() {
        return new EnemyHurtState(config, context, this);
    }

}
