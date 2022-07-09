using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour {
    public State previousState;
    public State currentState;

    void Update() {
        currentState.UpdateState();
    }

    public abstract void ForceHurt();
    public abstract void ForceDead();
    public virtual void ForceStunned(){}
}