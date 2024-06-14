using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTheTarget : IStrategy
{
    private IEnemyMovement owner;
    private Animator animator;

    private Transform target;
    private EnemyShooter ownerShooter;

    private Vector3 offSet;

    BehaviourState shootState = BehaviourState.SHOOTING;
    BehaviourState reloadingState = BehaviourState.RELOADING;

    public ShootTheTarget(IEnemyMovement owner, Transform target, Animator animator, EnemyShooter ownerShooter)
    {
        this.owner = owner;
        this.animator = animator;   
        this.target = target;
        this.ownerShooter = ownerShooter;

        offSet = new Vector3(0, 1.2f);
    }
    public ShootTheTarget(IEnemyMovement owner)
    {
        this.owner = owner;


    }
    public Node.Status Process(ref BehaviourState currentState)
    {
        if (owner != null)
        {
            owner.StopMovement();

            owner.SetFaceDirection(target);
            if (animator) animator.SetBool("CanShoot", true);
            Vector3 direction = target.position - (owner.GetLocation() + offSet);
            direction.Normalize();
            if (ownerShooter.Shoot(owner.GetLocation() + offSet, direction, ownerShooter.WeaponData.TypeOfShoot))
            {
                currentState = reloadingState;
            }
            else currentState = shootState;

        }
        return Node.Status.Success;
    }


    
}
