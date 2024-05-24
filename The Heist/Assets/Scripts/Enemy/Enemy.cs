using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform targetPosition;

    


    public void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveToTarget(Transform target)
    {
        if (navMeshAgent != null && target != null)
        {
            targetPosition = target;
            navMeshAgent.SetDestination(targetPosition.position);
            navMeshAgent.speed = 3;
        }
    }
    void Update()
    {
        if (targetPosition != null && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            targetPosition = null;
        }
    }
    public void ClearDestination()
    {
        targetPosition = null;
        navMeshAgent.speed = 0;
    }
}
