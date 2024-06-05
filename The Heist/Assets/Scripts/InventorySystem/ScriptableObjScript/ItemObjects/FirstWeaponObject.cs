using UnityEngine;

[CreateAssetMenu(fileName = "Default FirstWeapon Object", menuName ="Inventory System/Items/First Weapon")]
public class FirstWeaponObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.FirstWeapon;
    }
}