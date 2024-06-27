using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum GroundEnemyControllerStatus
{
    moving,
    hittedJump,
    dying,
    dead
}


public class GroundMovement : MonoBehaviour, IEnemyMovement
{
    #region PrivateAttributes
    protected Rigidbody myRigidbody;
    private CapsuleCollider myCollider;
    private NavMeshAgent navMesh;
    private float movementSpeed;
    private float jumpForce;
    private Vector3 inputDirection;

    private Vector3 preHitVelocity;
    private GroundEnemyControllerStatus status;

    private Animator animator;
    #endregion

    #region Properties
    public Vector3 InputDirection
    {
        get { return inputDirection; }
    }
    public float MovementSpeed
    {
        get { return movementSpeed; }
        set
        {
            if (value < 0)
            {
                movementSpeed = 0;
                return;
            }
            movementSpeed = value;
        }
    }
    public float JumpForce
    {
        get { return jumpForce; }
        set
        {
            if (value < 0)
            {
                jumpForce = 0;
                return;
            }
            jumpForce = value;
        }
    }
    public bool FaceDirection
    {
        get;
        set;
    }
    public bool IsStunned
    {
        get;
        set;
    }
    #endregion

    #region Unity_Game_Loop
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ResetMe();
    }

    private void FixedUpdate()
    {
        switch (status)
        {
            case GroundEnemyControllerStatus.dying:

                break;
        }
       
    }
    #endregion

    #region EnemyMovement

    public void Die(Vector2 dieForce, Vector3 sourcePosition)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        StopMovement();
        myCollider.enabled = false;
        
        animator.SetTrigger("Dead");
    }

    public void Hitted(Vector2 hitForce, Vector3 sourcePosition)
    {
        throw new System.NotImplementedException();
    }

    public void Hitted()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void ReverseInputDirection()
    {
        throw new System.NotImplementedException();
    }

    public void SetFaceDirection(Transform targetTransform)
    {
        Vector3 targetDirection = targetTransform.position - transform.position;

        if (targetDirection.magnitude < Mathf.Epsilon) return;

        targetDirection.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        float damping = 5.0f; 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * damping);

    }

    public void SetInputDirection(Vector3 inputDirection)
    {
        throw new System.NotImplementedException();
    }

    public void SetJumpForce(float jumpForce)
    {
        throw new System.NotImplementedException();
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        throw new System.NotImplementedException();
    }

    public void SetVerticalMovement(float speed)
    {
        throw new System.NotImplementedException();
    }

    public void StopMovement()
    {
        navMesh.ResetPath();
        navMesh.isStopped = true;

    }

    public void Teleport(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void ResetMe()
    {

    }

    public void MoveToTarget(Vector3 target, float speed)
    {
        if (navMesh)
        {
            navMesh.SetDestination(target);
            navMesh.speed = speed;
        }
    }

    public Vector3 GetLocation()
    {
        return myCollider.transform.position;
    }

    #endregion
}
