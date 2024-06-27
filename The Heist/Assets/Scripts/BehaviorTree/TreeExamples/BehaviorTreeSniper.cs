using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeSniper : MonoBehaviour
{
    private BehaviourTree tree;

    private IEnemyMovement ownerMovement;
    private Animator animator;
    private EnemyShooter ownerShooter;

    private WaveMenager waveMenager;
    
    private SpotMenager spotMenager;

    private GameObject player;

    private bool isDead;

    [SerializeField] private List<Spot> spots;
    [SerializeField] float maxDistanceToShoot = 20;
    [SerializeField] float speed = 5;
    [SerializeField] private bool isStunned;
    [SerializeField] Transform gunTransform;

    private Spot spot;

    public List<Spot> Spots
    {
        get { return spots; }
        set { spots = value;  }
    }

    private void Start()
    {
        ownerMovement = GetComponent<EnemyComponent>().GetEnemyMovement();
        animator = GetComponent<Animator>();
        ownerShooter = GetComponent<EnemyShooter>();

        waveMenager = GameObject.FindObjectOfType<WaveMenager>();
        
        spotMenager =  GameObject.Find("SpotMenager").GetComponent<SpotMenager>();

        Spots = spotMenager.Spots;
        spot = GetSpot();


        player = GameObject.Find("Player");       


        tree = new BehaviourTree("Sniper");

        Sequence MoveToSpot = new Sequence("MoveToSpot");

        MoveToSpot.AddChild(new Leaf("CanMoveToSpot", new Condition(CanMoveToSpot)));
        MoveToSpot.AddChild(new Leaf("MoveToSpot", new FollowTarget(ownerMovement,spot.SpotPosition(), speed, animator, false), behaviorTree: tree));


        Sequence stunned = new Sequence("Stunned");
        stunned.AddChild(new Leaf("CanStun?", new Condition(() => CanStun())));
        stunned.AddChild(new Leaf("Stunned", new Stunned(animator, ownerMovement), behaviorTree: tree));

        Sequence Shoot = new Sequence("ShootPlayer");
        Shoot.AddChild(new Leaf("CanShoot?", new CanShootTheTarget(transform, player.transform, maxDistanceToShoot, 0)));
        Shoot.AddChild(new Leaf("Shoot", new ShootTheTarget(ownerMovement, player.transform, gunTransform, animator,ownerShooter), behaviorTree: tree));

        Sequence Crouch = new Sequence("Crouch");
        Crouch.AddChild(new Leaf("Crouch", new Crouch(ownerMovement, animator), behaviorTree: tree));

        Selector selector = new Selector("Selector");

        selector.AddChild(stunned);
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
        float dist = Vector3.Distance(transform.position, spot.transform.position);
        if (dist <= 5)
        {
            //Check when is true
            return false;
        }
        return true;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            tree.Process();
        }
        //Debug.Log(tree.CurrentState.ToString());
    }
    bool CanStun()
    {
        // return ownerMovement.IsStunned;
        if (isStunned) return true;

        if (tree.currentState == BehaviourState.STUNNED) animator.SetBool("Stunned", false);

        return false;
    }
    public void EndAnimationReload(string pippo)
    {
       tree.currentState = BehaviourState.END;
    }

    public void onDeath(string empty)
    {
        if (isDead) return;
        isDead = true;
        ownerMovement.Die(); 
        waveMenager.EnemyDied();

    }
}
