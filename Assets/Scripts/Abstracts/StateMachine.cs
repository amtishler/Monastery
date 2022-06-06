using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State previousState;
    public State currentState;

    void Update()
    {
        currentState.UpdateState();
    }
}



// public abstract class StateMachine : MonoBehaviour
// {
//     public State previousState;
//     public State currentState;


//     // Update method, called once per frame.
//     void Update()
//     {
//         RunStateMachine();
//     }


//     // Reassigns & runs our current state.
//     private void RunStateMachine()
//     {
//         // State nextState = currentState?.RunCurrentState();
//         // if(nextState != null) SwitchState(nextState);
//     }


//     // Helper function to reassign state.
//     private void SwitchState(State nextState)
//     {
//         previousState = currentState;
//         currentState = nextState;
//     }
// }