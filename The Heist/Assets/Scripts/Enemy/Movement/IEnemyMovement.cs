using UnityEngine;

public enum EnemyMovementType  //Maybe we don't need a different Movement type
{
    ground
    //fly
}

public interface IEnemyMovement 
{

    void SetMovementSpeed(float movementSpeed);
    void SetInputDirection(Vector3 inputDirection);
    void ReverseInputDirection();
    void Jump(); 
    void StopMovement();
    void SetJumpForce(float jumpForce);
    void SetFaceDirection(bool value);
    void Hitted(Vector2 hitForce, Vector3 sourcePosition);
    void Hitted();
    void Die(Vector2 dieForce, Vector3 sourcePosition);
    void Die();
    void Teleport(Vector3 position);
    void SetVerticalMovement(float speed);
    void ResetMe();
    void MoveToTarget(Transform target, float speed);


    

    Vector3 InputDirection
    {
        get;
    }
    bool FaceDirection
    {
        get;
    }

}
