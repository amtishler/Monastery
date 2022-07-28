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