using UnityEngine;

public enum GrenadeType {
    Incendiary,
    Stun
}

public class GrenadeBase : MonoBehaviour, IGrenade
{
    [SerializeField] 
    private LayerMask destroyLayer;
    [SerializeField] 
    private Rigidbody rb;

    [SerializeField] 
    protected float radius;
    public virtual void Throw(Transform cameraView, Transform spawnTransform, float force, float throwUpwardForce)
    {
        gameObject.SetActive(true);
        transform.position = spawnTransform.position;
        Vector3 forceToAdd = cameraView.forward * force + Vector3.up * throwUpwardForce;
        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.collider.gameObject.layer) & destroyLayer.value) == 0) return;
    }
    
    public virtual void Explode(float radius, GrenadeType grenadeType)
    {
        Destroy();
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
