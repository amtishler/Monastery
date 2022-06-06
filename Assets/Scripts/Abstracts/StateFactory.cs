using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory
{
    protected StateMachine context;

    public StateFactory(StateMachine currentContext)
    {
        context = currentContext;
    }
}
