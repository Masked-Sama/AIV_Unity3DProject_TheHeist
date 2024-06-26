using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum BehaviourState
{
    IDLE,
    MOVING,
    SHOOTING,
    RELOADING,
    STUNNED,
    END
    // Add other possible states as needed
}

public class BehaviorTreePoliceOfficer : MonoBehaviour
{
    private BehaviourTree tree;

    private IEnemyMovement ownerMovement;
    private EnemyShooter ownerShooter;
    private Animator animator;

    private WaveMenager waveMenager;
    
    private GameObject player;

    private bool isDead;
    [SerializeField] private bool isStunned;

    //SerializeFields
    [SerializeField] bool canFollowThePlayer = true;
    [SerializeField] float maxDistanceToShoot = 20;
    [SerializeField] float minDistanceToFollow = 3f; // Adjust as needed
    [SerializeField] float speed = 5;

    private void Awake()
    {
        maxDistanceToShoot = Random.Range(8f, 16f);
        // Get all MeshRenderer components
        SkinnedMeshRenderer[] meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        // Filter meshes starting with "Character"
        List<SkinnedMeshRenderer> characterMeshes = new List<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
        {
            if (meshRenderer.name.StartsWith("Character"))
            {
                characterMeshes.Add(meshRenderer);
            }
        }

        // Randomly select a mesh index
        int randomIndex = Random.Range(0, characterMeshes.Count);

        // Activate the randomly selected mesh
        if (characterMeshes.Count > 0)
        {
            characterMeshes[randomIndex].enabled = true;
        }
        // Disable other meshes (optional)
        foreach (SkinnedMeshRenderer meshRenderer in characterMeshes)
        {
            if (meshRenderer != characterMeshes[randomIndex])
            {
                meshRenderer.enabled = false;
            }
        }
    }
    void Start()
    {
        ownerMovement = GetComponent<EnemyComponent>().GetEnemyMovement();
        animator = GetComponentInChildren<Animator>();
        ownerShooter = GetComponent<EnemyShooter>();

        waveMenager = GameObject.FindObjectOfType<WaveMenager>();
        
        player = GameObject.Find("Player");

        tree = new BehaviourTree("PoliceOfficer");

        var selector = new Selector("Idle||Follow");

        Sequence stunned = new Sequence("Stunned");
        stunned.AddChild(new Leaf("CanStun?", new Condition(() => CanStun())));
        stunned.AddChild(new Leaf("Stunned", new Stunned(animator, ownerMovement), behaviorTree: tree));

        Sequence Shoot = new Sequence("ShootPlayer");
        Shoot.AddChild(new Leaf("CanShoot?", new CanShootTheTarget(transform, player.transform, maxDistanceToShoot, minDistanceToFollow)));
        Shoot.AddChild(new Leaf("Shoot", new ShootTheTarget(ownerMovement, player.transform, animator, ownerShooter), behaviorTree: tree));

        Sequence Follow = new Sequence("FollowPlayer");
        Follow.AddChild(new Leaf("CanFollow?", new Condition(() => CanFollow())));
        Follow.AddChild(new Leaf("Follow", new FollowTarget(ownerMovement, player.transform, speed, animator, true), behaviorTree: tree));

        selector.AddChild(stunned);
        selector.AddChild(Shoot);
        selector.AddChild(Follow);
       // selector.AddChild(new Leaf("Idle", new StayInIdle(ownerMovement), behaviorTree: tree));



        tree.AddChild(selector);

    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
        tree.Process();
        }
        
        //Debug.Log(tree.CurrentState.ToString());
    }

    bool CanFollow()
    {
        if (tree.currentState == BehaviourState.RELOADING || tree.currentState == BehaviourState.IDLE) return false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        return (distance > maxDistanceToShoot && distance >= minDistanceToFollow);
    }

    bool CanStun()
    {
        // return ownerMovement.IsStunned;
        if (isStunned) return true;

        if (tree.currentState == BehaviourState.STUNNED) animator.SetBool("Stunned", false);

        return false;
    }

    public void EndAnimationReload(string empty)
    {
        tree.currentState = BehaviourState.END;
    }

    public void onDeath(string empty)
    {
        isDead = true;
        waveMenager.EnemyDied();
        ownerMovement.StopMovement();
    }
}
