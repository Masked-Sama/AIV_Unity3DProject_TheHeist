using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeThrow : MonoBehaviour, IPoolRequester
{

    [Header("Variables References")] 
    [SerializeField]
    private Transform camera;
    [SerializeField] 
    private Transform attackPoint;
    [SerializeField] 
    private PoolData[] grenadeType;

    [Header("Settings")]
    [SerializeField] private float throwCooldown;

    [Header("Throwing")] 
    [SerializeField] private KeyCode throwKey = KeyCode.G;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;

    private GrenadeType currentGrenadeType;
    public PoolData[] Datas {
        get { return grenadeType; }
    }

    private float currentBulletCD;

    public void OnEnable()
    {
        InputManager.Player.ThrowGrenade.performed += Throw;
        currentGrenadeType = GrenadeType.Stun;
    }

    private void Throw(InputAction.CallbackContext e)
    {
        //Player mi passa un valore per determinare la granata
        
        //scelgo la pool in base al valore
        IGrenade grenadeComponent;
        switch (currentGrenadeType)
        {
            case GrenadeType.Incendiary:
                grenadeComponent = Pooler.Istance.GetPooledObject(grenadeType[0]).GetComponent<IGrenade>();
                Debug.Log("Lancio la granata incendiaria");
                break;
            case GrenadeType.Stun:
                grenadeComponent = Pooler.Istance.GetPooledObject(grenadeType[1]).GetComponent<IGrenade>();
                Debug.Log("Lancio la granata stordente");
                break;
            default: return;
        }
        //Get From Pool
        if (grenadeComponent == null)
        {
            Debug.Log("Non c'è niente");
            return;
        }
        grenadeComponent.Throw(camera, attackPoint, throwForce, throwUpwardForce);
    }
}
