using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorTreeClient : MonoBehaviour
{
    private BehaviourTree tree;

    private NavMeshAgent ownerAgent;

    [SerializeField]
    private GameObject player;
    [SerializeField] float maxDistanceToEscape = 20;
    [SerializeField] float speed = 5;
    [SerializeField] float desiredDistance;


    bool isEscaping = false;

    void Awake()
    {
        ownerAgent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree("Client");

        var selector = new Selector("Idle||RandomEscapeCrouch");

        var sequenceIdle = new Sequence("IdleSequence");
        var onEnter = new Leaf("OnEnterIdle", new ActionStrategy(OnEnterIdle));
        var idle = new Leaf("Idle", new StayInIdle());

        sequenceIdle.AddChild(onEnter);
        sequenceIdle.AddChild(idle);


        var sequence = new Sequence("Choose");
        sequence.AddChild(new Leaf("CanEscapeOrCrouch", new Condition(IsPlayerDistance)));
      
        var randomSelector = new RandomSelector("Escape||Crouch");
        randomSelector.AddChild(new Leaf("RunAway", new ActionStrategy(MoveToPosition)));
        randomSelector.AddChild(new Leaf("Crouch", new StayInIdle())); //NEED ANIMATOR

        sequence.AddChild(randomSelector);

        selector.AddChild(sequence);
        selector.AddChild(sequenceIdle);
        tree.AddChild(selector);

    }

    void Update()
    {
        tree.Process();
    }
    void OnEnterIdle()
    {
        isEscaping = false;
    }





    //DA INSERIRE IN SINGOLE CLASSI PER RIUTILIZZARLE
    bool IsPlayerDistance()
    {
        return (transform.position - player.transform.position).sqrMagnitude < maxDistanceToEscape * 2; //Da correggere!!!
    }
    void MoveToPosition()
    {
        Vector3 newPosition;

        if (FindNewPosition(transform.position, desiredDistance, out newPosition) && !isEscaping)
        {
            ownerAgent.SetDestination(newPosition);
            isEscaping = true;
        }
        else Debug.Log("Not found new position");
    }

    bool FindNewPosition(Vector3 referencePosition, float desiredDistance, out Vector3 newPosition)
    {
        newPosition = Vector3.zero;

        for (int attempts = 0; attempts < 20; attempts++)
        {
            // Sample a random position within the desired distance
            Vector3 randomOffset = Random.insideUnitSphere * desiredDistance;
            Vector3 samplePosition = referencePosition + randomOffset;

            // Check if the sampled position is valid on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(samplePosition, out hit, desiredDistance, NavMesh.AllAreas))
            {
                newPosition = hit.position;
                return true;
            }
        }

        return false; // No valid position found after maxAttempts
    }
}
