using UnityEngine;

[CreateAssetMenu(fileName = "New Ammunition Object", menuName = "Inventory System/Items/Ammunitions")]
public class AmmunitionObject : ItemObject
{
    [SerializeField]
    private int quantity;
    private void Awake()
    {
        type = ItemType.Ammunition;
    }
}