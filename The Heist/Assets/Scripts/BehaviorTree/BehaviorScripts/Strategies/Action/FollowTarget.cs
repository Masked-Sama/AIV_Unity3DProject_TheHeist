using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : StrategyAction
{
    private IEnemyMovement owner;


    private Transform target;
    private float speed;

    public FollowTarget(IEnemyMovement owner, Transform target, float speed)
    {
        this.owner = owner;
        this.target = target;
        this.speed = speed;
    }
    public FollowTarget()
    {

    }

    public override Node.Status Process()
    {
        if (owner != null) owner.MoveToTarget(target, speed);

        Debug.Log("FollowPlayer");
        return Node.Status.Success;
    }

}
