using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ====================
      SUPERCLASSES
==================== */

// PlayerState
public abstract class EnemyState : State {
    protected EnemyConfig config;
    protected EnemyStateMachine currentContext;
    protected EnemyStateFactory factory;

    public EnemyState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(currentContext) {
        this.config = config;
        this.currentContext = currentContext;
        this.factory = stateFactory;
    }
}


/* ====================
       SUBCLASSES
==================== */

// Idle
public class EnemyIdleState : EnemyState {

    public EnemyIdleState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if(CheckVision()) SwitchStates(factory.Aggressive());
    }

    private bool CheckVision()
    {
        Collider2D collider = Physics2D.OverlapCircle(config.transform.position, config.detectionradius, LayerMask.GetMask("Player Hurtbox"));
        if(collider == null) return false;
        else{
            config.target = collider.gameObject;
            return true;
        }
    }
}

// Aggressive
public class EnemyAggressiveState : EnemyState {

    public EnemyAggressiveState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        Debug.Log("GRRR");
    }

    public override void UpdateState() {
        config.MoveTowards(config.target);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {}
}

// Hurt
public class EnemyHurtState : EnemyState {

    private float deacceleration;
    private float recoverytimer;


    public EnemyHurtState(EnemyConfig config, EnemyStateMachine currentContext, EnemyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        deacceleration = config.RecoveryDeaccel;
        config.SlowDown(deacceleration);
    }

    public override void UpdateState()
    {
        deacceleration += config.RecoveryDeaccel * Time.deltaTime;
        config.SlowDown(deacceleration);
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates(){
        if(config.Speed == 0) SwitchStates(currentContext.previousState);
    }
}

