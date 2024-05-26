using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTheTarget : IStrategy
{
    private IEnemyMovement owner;

    public ShootTheTarget(IEnemyMovement owner)
    {
        this.owner = owner;
    }
    public Node.Status Process()
    {
        if (owner != null) owner.StopMovement();

        Debug.Log("Shoot");
        return Node.Status.Success;
    }
}
