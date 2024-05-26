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
    private BoxCollider myCollider;
    private NavMeshAgent navMesh;
    private float movementSpeed;
    private float jumpForce;
    private Vector3 inputDirection;

    private Vector3 preHitVelocity;
    private GroundEnemyControllerStatus status;
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
    #endregion

    #region Unity_Game_Loop
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponentInChildren<BoxCollider>();
        navMesh = GetComponent<NavMeshAgent>();
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
        throw new System.NotImplementedException();
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

    public void SetFaceDirection(bool value)
    {
        throw new System.NotImplementedException();
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
    }

    public void Teleport(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void ResetMe()
    {

    }

    public void MoveToTarget(Transform target, float speed)
    {
        if (navMesh)
        {
            navMesh.SetDestination(target.position);
            navMesh.speed = speed;
        }
    }

    #endregion
}
