using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTheTarget : IStrategy
{
    private IEnemyMovement owner;
    private Animator animator;

    private Transform target;

    BehaviourState state = BehaviourState.ATTACKING;

    public ShootTheTarget(IEnemyMovement owner, Transform target, Animator animator)
    {
        this.owner = owner;
        this.animator = animator;   
        this.target = target;
    }
    public ShootTheTarget(IEnemyMovement owner)
    {
        this.owner = owner;


    }
    public Node.Status Process(ref BehaviourState currentState)
    {
        if (owner != null)
        {
            owner.StopMovement();

            owner.SetFaceDirection(target);
            if (animator) animator.SetBool("CanShoot", true);
        }
        currentState = state;
        
        Debug.Log("Shoot");
        return Node.Status.Success;
    }
}
