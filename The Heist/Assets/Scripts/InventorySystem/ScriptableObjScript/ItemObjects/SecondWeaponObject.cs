using UnityEngine;

[CreateAssetMenu(fileName = "Default SecondWeapon Object", menuName = "Inventory System/Items/Second Weapon")]
public class SecondWeaponObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.SecondWeapon;
    }
}