using UnityEngine;

[CreateAssetMenu(fileName = "Default Consumable Object", menuName = "Inventory System/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}