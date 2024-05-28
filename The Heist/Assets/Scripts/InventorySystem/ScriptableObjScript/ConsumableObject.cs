using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumables")]
public class ConsumableObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}