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

    public State Readying() {
        return new EnemyReadyingState(config, currentContext, this);
    }

    public State Attack() {
        return new EnemyAttackState(config, currentContext, this);
    }

    public State Hurt() {
        return new EnemyHurtState(config, currentContext, this);
    }

    public State Dead() {
        return new EnemyDeadState(config, currentContext, this);
    }

    public State Grabbed() {
        return new EnemyGrabbedState(config, currentContext, this);
    }

    public State Stunned() {
        return new EnemyStunnedState(config, currentContext, this);
    }

    public State Projectile() {
        return new EnemyProjectileState(config, currentContext, this);
    }

}
