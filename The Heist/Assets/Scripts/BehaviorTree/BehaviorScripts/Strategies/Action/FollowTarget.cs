using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : StrategyAction
{
    private Enemy owner;

    private Transform target;

    public FollowTarget(Enemy owner, Transform target)
    {
        this.owner = owner;
        this.target = target;
    }
    public FollowTarget()
    {

    }

    public override Node.Status Process()
    {
        if (owner) owner.MoveToTarget(target);

        Debug.Log("FollowPlayer");
        return Node.Status.Success;
    }

}
