using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : MonoBehaviour, IDamageble
{
    private IEnemyMovement movementComponent;

    [SerializeField]
    private EnemyMovementType movementType;

    [SerializeField]
    private EnemyShooter shooterComponent;

    [SerializeField]
    private HealthModule healthModule;
    
    public IEnemyMovement GetEnemyMovement() { return movementComponent; }
    public EnemyShooter GetEnemyShooter() {  return shooterComponent; }

    public void Awake()
    {
        CreateEnemyMovement();
        healthModule.Reset();
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
    #region Interface IDamageble
    public void TakeDamage(DamageContainer damage)
    {
        InternalTakeDamage(damage);
        Debug.Log("EnemyHealth: " + healthModule.CurrentHP);
    }
    #endregion

    #region HealthModule
    private void InternalTakeDamage(DamageContainer damage)
    {
        healthModule.TakeDamage(damage);
    }


    #endregion
}
