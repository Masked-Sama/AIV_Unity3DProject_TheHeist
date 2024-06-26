
using UnityEngine;

public class FollowTarget : IStrategy
{
    private IEnemyMovement owner;


    private Vector3 target;
    private Transform targetTransform;
    private float speed;
    private float scatterRadius = 4;
    private Animator animator;

    BehaviourState state = BehaviourState.MOVING;

    private bool everyframe = true;

    public FollowTarget(IEnemyMovement owner, Vector3 target, float speed, Animator animator, bool everyFrame)
    {
        this.owner = owner;
        this.target = target;
        this.speed = speed;
        this.animator = animator;
        this.everyframe = everyFrame;
    }
    public FollowTarget(IEnemyMovement owner, Transform target, float speed, Animator animator, bool everyFrame)
    {
        this.owner = owner;
        this.targetTransform = target;
        this.speed = speed;
        this.animator = animator;
        this.everyframe = everyFrame;
    }


    public Node.Status Process(ref BehaviourState currentState)
    {
        if (!everyframe)
        {
            if (currentState == BehaviourState.MOVING) return Node.Status.Success;
        }

        //if (owner != null)
        // {
        if (target != Vector3.zero)
        {
            owner.MoveToTarget(target, speed);
        }
        else
        {
            Vector3 randomOffset = Random.insideUnitCircle * scatterRadius;
            owner.MoveToTarget(targetTransform.position + randomOffset, speed);
        }
        //if(animator) 
        animator.SetBool("CanShoot", false);
        currentState = state;
        // }

        //Debug.Log("FollowPlayer");
        return Node.Status.Success;
    }

}
