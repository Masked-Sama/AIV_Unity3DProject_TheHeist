using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBase : MonoBehaviour, IGrenade
{
    [SerializeField] 
    private LayerMask destroyLayer;
    [SerializeField] 
    private Rigidbody rb;
    
    public void Throw(Transform cameraView, Transform spawnTransform, float force, float throwUpwardForce)
    {
        gameObject.SetActive(true);
        transform.position = spawnTransform.position;
        Vector3 forceToAdd = cameraView.transform.forward * force + transform.up * throwUpwardForce;
        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.collider.gameObject.layer) & destroyLayer.value) == 0) return;
        Explode();
    }

    public virtual void Explode()
    {
        //Exploding
        
        Destroy();    
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
