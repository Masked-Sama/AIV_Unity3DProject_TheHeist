using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

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
        if(healthModule.IsDead)
        {
            movementComponent.Die();
            Rigidbody b = gameObject.GetComponent<Rigidbody>();
            b.useGravity = false;
            foreach (var e in gameObject.GetComponents<Collider>())
            {
                e.enabled = false;
            }

        }

    }


    #endregion

    #region Mono
    private void Awake()
    {
        CreateEnemyMovement();
        healthModule.Reset();
    }
    #endregion
}
