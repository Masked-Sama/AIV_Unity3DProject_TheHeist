using System.Collections.Generic;
using UnityEngine;

public class GenericDamager : MonoBehaviour, IDamager {
    
    [SerializeField]
    private DamageContainer damage;
    [SerializeField]
    private List<string> damagebleTags;

    private void OnTriggerStay (Collider other) {
        foreach (string tag in damagebleTags)
        {
            if (!other.CompareTag(tag)) continue;
            IDamageble damageble = other.gameObject.GetComponent<IDamageble>();
            if (damageble == null) return;
            damage.SetContactPoint(other.ClosestPoint(transform.position));
            damageble.TakeDamage(damage);
        }
        
       
    }

}
