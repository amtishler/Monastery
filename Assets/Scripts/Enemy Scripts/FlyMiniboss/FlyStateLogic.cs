using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ====================
      SUPERCLASSES
==================== */

// PlayerState
public abstract class FlyState : State {
    protected FlyConfig config;
    protected FlyStateMachine currentContext;
    protected FlyStateFactory factory;

    public FlyState(FlyConfig config, FlyStateMachine currentContext, FlyStateFactory stateFactory)
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
public class FlyIdleState : FlyState {

    public FlyIdleState(FlyConfig config, FlyStateMachine currentContext, FlyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if(!config.grounded) SwitchStates(factory.Falling());
        else if(config.attacking) SwitchStates(factory.Aggressive());
    }

}

public class FlyAggressiveState : FlyState {

    public FlyAggressiveState(FlyConfig config, FlyStateMachine currentContext, FlyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {
        config.Move(config.target);
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
    }
}

// Hurt
public class FlyHurtState : FlyState {

    private float deacceleration;
    private float recoverytimer;


    public FlyHurtState(FlyConfig config, FlyStateMachine currentContext, FlyStateFactory stateFactory)
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
        if(!config.grounded) SwitchStates(factory.Falling());
        if(config.Speed == 0){
            if(config.target != null) SwitchStates(factory.Aggressive());
            else SwitchStates(factory.Idle());
        }
    }
}

// Stunned
public class FlyStunnedState : FlyState {

    public FlyStunnedState(FlyConfig config, FlyStateMachine currentContext, FlyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {
        config.stunned = true;
        Renderer renderer = config.GetComponent<Renderer>();
        renderer.material.SetInt("_Active", 1);
    }

    public override void UpdateState() {
        if(config.Speed > 0){
            config.SlowDown(config.RecoveryDeaccel);
        }
        CheckSwitchStates();
    }

    public override void ExitState() {
        config.Stun = 0;
        config.stunned = false;
        Renderer renderer = config.GetComponent<Renderer>();
        renderer.material.SetInt("_Active", 0);
    }

    public override void CheckSwitchStates() {
    }
}

public class FlyFallingState : FlyState {

    public FlyFallingState(FlyConfig config, FlyStateMachine currentContext, FlyStateFactory stateFactory)
    : base(config, currentContext, stateFactory){}

    public override void EnterState() {}

    public override void UpdateState() {
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
    }

}