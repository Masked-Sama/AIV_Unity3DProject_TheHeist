using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorTreeFollowPlayer : MonoBehaviour
{
    private BehaviourTree tree;

    //private NavMeshAgent agent;

    //TODO: Animation, Create an Enemy for NavMesh

    //ONLY FOR DEBUG 
    [SerializeField] bool canFollowThePlayer = true; 


    void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree("FollowPlayer");

        var selector = new Selector("Idle||Follow");

        Sequence Follow = new Sequence("FollowPlayer");
        Follow.AddChild(new Leaf("CanFollow?", new Condition(() => CanFollow())));
        //Follow.AddChild(new Leaf("CanFollow?", new MoveToTarget()));
        Follow.AddChild(new Leaf("Follow", new FollowTarget()));

        selector.AddChild(Follow);
        selector.AddChild(new Leaf("Idle", new StayInIdle(false)));

        tree.AddChild(selector);
        
    }

    private void Update()
    {
        tree.Process();
    }

    bool CanFollow()
    {
        return canFollowThePlayer;
    }

}
