using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName ="Inventory System/Items/Weapons")]
public class WeaponObject: ItemObject
{
    private void Awake()
    {
        type = ItemType.Weapon;
    }
}