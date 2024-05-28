using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StayInIdle : IStrategy
{
    private IEnemyMovement owner;
    BehaviourState state = BehaviourState.IDLE;

    public StayInIdle(IEnemyMovement owner = null) 
    {
        if (owner != null)
            this.owner = owner;
    }

    public Node.Status Process(ref BehaviourState currentState)
    {
        if (currentState == BehaviourState.IDLE) return Node.Status.Success;

        currentState = state;
        if (owner != null) owner.StopMovement();
        Debug.Log("Stay in Idle"); 

        return Node.Status.Success;
    }

}
