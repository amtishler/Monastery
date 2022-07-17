using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {

    protected StateMachine ctx;
    public string name = "None";

    // Constructor
    public State(StateMachine currentContext) {
        ctx = currentContext;
    }

    // Switches to new state
    public void SwitchStates(State newState) {
        ctx.previousState = this;
        ExitState();
        newState.EnterState();
        ctx.currentState = newState;
    }

    void UpdateStates(){}

    // Abstract methods
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
}