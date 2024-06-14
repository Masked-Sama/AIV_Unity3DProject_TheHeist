using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviourState
{
    IDLE,
    MOVING,
    SHOOTING,
    RELOADING,
    END
    // Add other possible states as needed
}

public class BehaviorTreePoliceOfficer : MonoBehaviour
{
    private BehaviourTree tree;

    private IEnemyMovement ownerMovement;
    private EnemyShooter ownerShooter;
    private Animator animator;


    
    private GameObject player;

    //SerializeFields
    [SerializeField] bool canFollowThePlayer = true;
    [SerializeField] float maxDistanceToShoot = 20;
    [SerializeField] float speed = 5;

    void Start()
    {
        ownerMovement = GetComponent<EnemyComponent>().GetEnemyMovement();
        animator = GetComponentInChildren<Animator>();
        ownerShooter = GetComponent<EnemyShooter>();

        player = GameObject.Find("Player");

        tree = new BehaviourTree("PoliceOfficer");

        var selector = new Selector("Idle||Follow");

        Sequence Shoot = new Sequence("ShootPlayer");
        Shoot.AddChild(new Leaf("CanShoot?", new CanShootTheTarget(transform, player.transform, maxDistanceToShoot)));
        Shoot.AddChild(new Leaf("Shoot", new ShootTheTarget(ownerMovement, player.transform, animator, ownerShooter), behaviorTree: tree));

        Sequence Follow = new Sequence("FollowPlayer");
        Follow.AddChild(new Leaf("CanFollow?", new Condition(() => CanFollow())));
        Follow.AddChild(new Leaf("Follow", new FollowTarget(ownerMovement, player.transform, speed, animator, true), behaviorTree: tree));

        selector.AddChild(Shoot);
        selector.AddChild(Follow);
       // selector.AddChild(new Leaf("Idle", new StayInIdle(ownerMovement), behaviorTree: tree));



        tree.AddChild(selector);

    }

    private void FixedUpdate()
    {
        tree.Process();
        Debug.Log(tree.CurrentState.ToString());
    }

    bool CanFollow()
    {
        if (tree.currentState == BehaviourState.RELOADING || tree.currentState == BehaviourState.IDLE) return false;
                
        return true;
    }

    public void Pippo(string pippo)
    {
        tree.currentState = BehaviourState.END;
    }
}
