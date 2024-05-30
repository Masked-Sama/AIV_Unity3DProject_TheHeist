using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviourState
{
    IDLE,
    MOVING,
    ATTACKING,
    END
    // Add other possible states as needed
}

public class BehaviorTreePoliceOfficer : MonoBehaviour
{
    private BehaviourTree tree;

    private IEnemyMovement ownerMovement;
    private Animator animator;


    [SerializeField]
    private GameObject player;

    //SerializeFields
    [SerializeField] bool canFollowThePlayer = true; 
    [SerializeField] float maxDistanceToShoot = 20; 
    [SerializeField] float speed = 5; 


    void Start()
    {
        ownerMovement = GetComponent<EnemyComponent>().GetEnemyMovement();
        animator = GetComponentInChildren<Animator>();

        tree = new BehaviourTree("PoliceOfficer");

        var selector = new Selector("Idle||Follow");

        Sequence Shoot = new Sequence("ShootPlayer");
        Shoot.AddChild(new Leaf("CanShoot?", new CanShootTheTarget(transform,player.transform, maxDistanceToShoot)));
        Shoot.AddChild(new Leaf("Shoot", new ShootTheTarget(ownerMovement, player.transform, animator), behaviorTree: tree));

        Sequence Follow = new Sequence("FollowPlayer");
        Follow.AddChild(new Leaf("CanFollow?", new Condition(() => CanFollow())));
        Follow.AddChild(new Leaf("Follow", new FollowTarget(ownerMovement, player.transform, speed, animator, true), behaviorTree: tree));

        selector.AddChild(Shoot);
        selector.AddChild(Follow);
        selector.AddChild(new Leaf("Idle", new StayInIdle(ownerMovement), behaviorTree: tree));



        tree.AddChild(selector);
        
    }

    private void Update()
    {
        tree.Process();
        Debug.Log(tree.CurrentState.ToString());
    }

    bool CanFollow()
    {
        return canFollowThePlayer;
    }

}
