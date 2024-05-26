using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BehaviorTreePoliceOfficer : MonoBehaviour
{
    private BehaviourTree tree;

    private IEnemyMovement ownerMovement;


    [SerializeField]
    private GameObject player;

    //TODO: Animation

    //SerializeFields
    [SerializeField] bool canFollowThePlayer = true; 
    [SerializeField] float maxDistanceToShoot = 20; 
    [SerializeField] float speed = 5; 


    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();

        ownerMovement = GetComponent<EnemyComponent>().GetEnemyMovement();

        tree = new BehaviourTree("FollowPlayer");

        var selector = new Selector("Idle||Follow");

        Sequence Shoot = new Sequence("ShootPlayer");
        Shoot.AddChild(new Leaf("CanShoot?", new CanShootTheTarget(transform,player.transform, maxDistanceToShoot)));
        Shoot.AddChild(new Leaf("Shoot", new ShootTheTarget(ownerMovement)));

        Sequence Follow = new Sequence("FollowPlayer");
        Follow.AddChild(new Leaf("CanFollow?", new Condition(() => CanFollow())));
        Follow.AddChild(new Leaf("Follow", new FollowTarget(ownerMovement, player.transform, speed)));

        selector.AddChild(Shoot);
        selector.AddChild(Follow);
        selector.AddChild(new Leaf("Idle", new StayInIdle(false, ownerMovement)));



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
