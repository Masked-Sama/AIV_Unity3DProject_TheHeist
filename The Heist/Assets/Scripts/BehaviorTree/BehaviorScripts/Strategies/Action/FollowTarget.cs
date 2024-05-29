using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class FollowTarget : IStrategy
{
    private IEnemyMovement owner;


    private Transform target;
    private float speed;

    private Animator animator;

    BehaviourState state = BehaviourState.MOVING;

    private bool everyframe = true;

    public FollowTarget(IEnemyMovement owner, Transform target, float speed, Animator animator, bool everyFrame)
    {
        this.owner = owner;
        this.target = target;
        this.speed = speed;
        this.animator = animator;
        this.everyframe = everyFrame;
    }


    public Node.Status Process(ref BehaviourState currentState)
    {
        if(!everyframe)
        {
        if (currentState == BehaviourState.MOVING) return Node.Status.Success;
        }

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
