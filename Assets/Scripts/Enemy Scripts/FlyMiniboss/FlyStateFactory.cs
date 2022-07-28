using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStateFactory : StateFactory
{
    FlyConfig config;
    FlyStateMachine currentContext;
    
    public FlyStateFactory(FlyConfig config, FlyStateMachine currentContext)
    : base(currentContext) {
        this.config = config;
        this.currentContext = currentContext;
    }

    public State Idle() {
        return new FlyIdleState(config, currentContext, this);
    }

    public State Aggressive() {
        return new FlyAggressiveState(config, currentContext, this);
    }

    public State Falling() {
        return new FlyFallingState(config, currentContext, this);
    }

}
