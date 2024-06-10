using UnityEngine;

public class IncendiaryGrenade : GrenadeBase
{
    [SerializeField]
    private PoolData fireZones;
    private Vector3 zoneSize;
    
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        Explode(radius, GrenadeType.Incendiary);
    }

    public override void Explode(float radius, GrenadeType grenadeType)
    {
        GameObject zone = Pooler.Istance.GetPooledObject(fireZones);
        zone.transform.position = transform.position; 
        zone.transform.localScale = zoneSize = new Vector3(radius, 0.05f, radius);
        zone.SetActive(true);
        Physics.OverlapSphere(transform.position, radius);
        base.Explode(radius, grenadeType);
    }
}
