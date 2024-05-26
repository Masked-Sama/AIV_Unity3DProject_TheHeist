using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : MonoBehaviour
{
    private IEnemyMovement movementComponent;

    [SerializeField]
    private EnemyMovementType movementType;
    
    public IEnemyMovement GetEnemyMovement() { return movementComponent; }

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
