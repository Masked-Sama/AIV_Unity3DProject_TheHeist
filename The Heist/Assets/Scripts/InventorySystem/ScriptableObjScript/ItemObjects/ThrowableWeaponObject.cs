using UnityEngine;

[CreateAssetMenu(fileName = "Default ThrowableWeapon Object", menuName = "Inventory System/Items/Throwable Weapon")]
public class ThrowableWeaponObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.ThrowableWeapon;
    }
}