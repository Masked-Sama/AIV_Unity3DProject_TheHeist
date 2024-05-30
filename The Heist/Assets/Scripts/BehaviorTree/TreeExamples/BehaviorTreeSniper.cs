using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeSniper : MonoBehaviour
{
    private BehaviourTree tree;

    private IEnemyMovement ownerMovement;
    private Animator animator;


    [SerializeField] private GameObject player;

    [SerializeField] private List<Spot> spots;
    [SerializeField] float maxDistanceToShoot = 20;
    [SerializeField] float speed = 5;


    private Spot spot;

    private void Start()
    {
        ownerMovement = GetComponent<EnemyComponent>().GetEnemyMovement();
        animator = GetComponent<Animator>();

        spot = GetSpot();

        tree = new BehaviourTree("Sniper");

        Sequence MoveToSpot = new Sequence("MoveToSpot");

        MoveToSpot.AddChild(new Leaf("CanMoveToSpot", new Condition(CanMoveToSpot)));
        MoveToSpot.AddChild(new Leaf("MoveToSpot", new FollowTarget(ownerMovement,spot.transform, speed, animator, false), behaviorTree: tree));

        Sequence Shoot = new Sequence("ShootPlayer");
        Shoot.AddChild(new Leaf("CanShoot?", new CanShootTheTarget(transform, player.transform, maxDistanceToShoot)));
        Shoot.AddChild(new Leaf("Shoot", new ShootTheTarget(ownerMovement, player.transform,animator), behaviorTree: tree));

        Sequence Crouch = new Sequence("Crouch");
        Crouch.AddChild(new Leaf("Crouch", new Crouch(ownerMovement, animator), behaviorTree: tree));

        Selector selector = new Selector("Selector");

        selector.AddChild(MoveToSpot);
        selector.AddChild(Shoot);
        selector.AddChild(Crouch);

        tree.AddChild(selector);
    }


    private Spot GetSpot()
    {
        foreach (var spot in spots)
        {
            if (spot.IsFree)
            {
            spot.IsFree = false;
            return spot;
            }
                
        }
        return null;
    }

    private bool CanMoveToSpot()
    {        
        if(Vector3.Distance(transform.position, spot.transform.position) <= 3) return false;

        return true;
    }

    private void Update()
    {
        tree.Process();
        Debug.Log(tree.CurrentState.ToString());
    }

}
