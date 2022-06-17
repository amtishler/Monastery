using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ====================
      SUPERCLASSES
==================== */

// PlayerState
public abstract class EnemyState : State {
    protected EnemyConfig config;
    protected EnemyStateFactory factory;

    public EnemyState(EnemyConfig config, StateMachine currentContext, EnemyStateFactory stateFactory)
    : base(currentContext) {
        this.config = config;
        this.factory = stateFactory;
    }
}


/* ====================
       SUBCLASSES
==================== */

// Idle
public class EnemyIdleState : EnemyState {

    public EnemyIdleState(EnemyConfig config, StateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {}

    public override void InitializeSubState() {}
}

// Aggressive
public class EnemyAggressiveState : EnemyState {

    public EnemyAggressiveState(EnemyConfig config, StateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {}

    public override void ExitState() {}

    public override void CheckSwitchStates() {}

    public override void InitializeSubState() {}
}

// Hurt
public class EnemyHurtState : EnemyState {

    private float deacceleration;

    public EnemyHurtState(EnemyConfig config, StateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        deacceleration = config.Recoverydeaccel;
        config.SlowDown(deacceleration);
    }

    public override void UpdateState()
    {
        deacceleration += config.Recoverydeaccel * Time.deltaTime;
        config.SlowDown(deacceleration);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates(){
        if(config.Speed == 0) SwitchStates(factory.Idle());
    }

    public override void InitializeSubState() {}
}

