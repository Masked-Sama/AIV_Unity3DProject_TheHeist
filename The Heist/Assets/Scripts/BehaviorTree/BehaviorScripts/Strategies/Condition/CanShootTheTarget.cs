using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanShootTheTarget : Condition
{
    private readonly Func<bool> condition;

    private readonly Transform ownerTransform;
    private readonly Transform targetTransform;
   // private readonly float maxDistanceSquared; // Store maxDistance squared for comparison

    private readonly float maxDistanceSquared; // Store maxDistance squared for comparison
    private readonly float minDistanceSquared; // Store minDistanceSquared for comparison

    public CanShootTheTarget(Transform ownerTransform, Transform targetTransform, float maxDistance, float minDistance)
    {
        this.ownerTransform = ownerTransform;
        this.targetTransform = targetTransform;
        //this.maxDistanceSquared = maxDistance * maxDistance; // Calculate maxDistance squared once

        this.maxDistanceSquared = maxDistance * maxDistance;
        this.minDistanceSquared = minDistance * minDistance; // Calculate minDistance squared once

        bool CanShoot()
        {
            //return (ownerTransform.position - targetTransform.position).sqrMagnitude <= maxDistanceSquared;

            float distanceSquared = (ownerTransform.position - targetTransform.position).sqrMagnitude;
            return (distanceSquared <= maxDistanceSquared && distanceSquared > minDistanceSquared);
        }

        condition = CanShoot;
        base.predicate = condition;
    }

    // No need to override Process() here as the base class implementation works
}