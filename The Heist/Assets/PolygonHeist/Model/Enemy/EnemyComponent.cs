using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : MonoBehaviour
{
    private IEnemyMovement movementComponent;

    [SerializeField]
    private EnemyMovementType movementType;

    [SerializeField]
    private EnemyShooter shooterComponent;
    
    public IEnemyMovement GetEnemyMovement() { return movementComponent; }
    public EnemyShooter GetEnemyShooter() {  return shooterComponent; }

    public void Awake()
    {
        CreateEnemyMovement();
    }

    private void CreateEnemyMovement()
    {
        switch (movementType)
        {
            case EnemyMovementType.ground:
                movementComponent = gameObject.AddComponent<GroundMovement>();
                break;

        }
    }


}
