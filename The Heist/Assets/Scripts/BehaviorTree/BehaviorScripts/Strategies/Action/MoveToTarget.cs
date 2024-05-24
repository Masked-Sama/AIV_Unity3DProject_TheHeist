using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : StrategyAction
{
    readonly Transform owner;
    readonly NavMeshAgent agent;
    readonly Transform target;

    public MoveToTarget(Transform entity, NavMeshAgent agent, Transform target)
    {
        this.owner = entity;
        this.agent = agent;
        this.target = target;
    }
    public MoveToTarget() { }

    public override Node.Status Process()
    {
        if (Vector3.Distance(owner.position, target.position) < 1f)
        {
            return Node.Status.Success;
        }

        agent.SetDestination(target.position);
        owner.LookAt(target.position.With(y: owner.position.y));

        return Node.Status.Running;
    }

}
