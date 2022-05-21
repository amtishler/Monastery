using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State currentState;

    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        //Runs only if currentState is not null
        State nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchState(nextState);
        }
    }

    private void SwitchState(State nextState)
    {
        currentState = nextState;
    }
}
