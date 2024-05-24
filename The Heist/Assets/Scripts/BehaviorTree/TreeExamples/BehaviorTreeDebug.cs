using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BehaviorTreeDebug : MonoBehaviour
{
    private BehaviourTree tree;

    private Enemy owner;

    [SerializeField]
    private GameObject target;
    //private NavMeshAgent agent;

    //TODO: Animation, Create an Enemy for NavMesh

    //ONLY FOR DEBUG 
    [SerializeField] bool canFollowThePlayer = true; 


    void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();

        owner = GetComponent<Enemy>();

        tree = new BehaviourTree("FollowPlayer");

        var selector = new Selector("Idle||Follow");

        Sequence Follow = new Sequence("FollowPlayer");
        Follow.AddChild(new Leaf("CanFollow?", new Condition(() => CanFollow())));
        //Follow.AddChild(new Leaf("CanFollow?", new MoveToTarget()));
        Follow.AddChild(new Leaf("Follow", new FollowTarget(owner,target.transform)));

        selector.AddChild(Follow);
        selector.AddChild(new Leaf("Idle", new StayInIdle(false, owner)));

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
