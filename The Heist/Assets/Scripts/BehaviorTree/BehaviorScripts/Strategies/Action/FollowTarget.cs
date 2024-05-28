using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : StrategyAction
{
    private IEnemyMovement owner;


    private Transform target;
    private float speed;

    private Animator animator;

    BehaviourState state = BehaviourState.MOVING;

    public FollowTarget(IEnemyMovement owner, Transform target, float speed, Animator animator)
    {
        this.owner = owner;
        this.target = target;
        this.speed = speed;
        this.animator = animator;
    }   
    public FollowTarget()
    {
        
    }


    public override Node.Status Process(ref BehaviourState currentState)
    {
        if (owner != null)
        {
            owner.MoveToTarget(target, speed);
            if(animator) animator.SetBool("CanShoot", false);
            currentState = state;
        }

        Debug.Log("FollowPlayer");
        return Node.Status.Success;
    }

}
