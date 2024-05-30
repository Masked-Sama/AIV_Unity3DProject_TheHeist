using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Crouch : IStrategy
{
    private IEnemyMovement owner;
    BehaviourState state = BehaviourState.IDLE;

    private Animator animator;

    public Crouch(IEnemyMovement owner, Animator animator) 
    {
        if (owner != null)
            this.owner = owner;

        this.animator = animator;
    }

    public Node.Status Process(ref BehaviourState currentState)
    {
        if (currentState == BehaviourState.IDLE) return Node.Status.Success;

        currentState = state;
        if (owner != null) owner.StopMovement();
        animator.SetTrigger("Crouch");
        animator.SetBool("CanShoot", false);
        Debug.Log("Stay in Idle"); 

        return Node.Status.Success;
    }

}
