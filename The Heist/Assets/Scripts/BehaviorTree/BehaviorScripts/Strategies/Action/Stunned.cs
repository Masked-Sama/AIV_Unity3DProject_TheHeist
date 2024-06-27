using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stunned : IStrategy
{
    private IEnemyMovement owner;
    BehaviourState state = BehaviourState.STUNNED;

    private Animator animator;

    public Stunned(Animator animator,IEnemyMovement owner = null) 
    {
        if (owner != null)
            this.owner = owner;
        this.animator = animator;
    }

    public Node.Status Process(ref BehaviourState currentState)
    {
        if (currentState == BehaviourState.STUNNED) return Node.Status.Success;

        currentState = state;
        if (owner != null)
        {
            owner.StopMovement();
            animator.SetBool("Stunned", true);

        }
        Debug.Log("Stunned"); 

        return Node.Status.Success;
    }

}
