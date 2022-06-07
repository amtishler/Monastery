using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {

    protected StateMachine ctx;


    // Constructor
    public State(StateMachine currentContext) {
        ctx = currentContext;
    }

    // Switches to new state
    protected void SwitchStates(State newState) {
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
    public abstract void InitializeSubState();
}