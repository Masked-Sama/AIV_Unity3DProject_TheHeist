using UnityEngine;
public class ConsumableData : ItemData
{
    [SerializeField] private float hp;
    public float HP { get { return hp; } }
}