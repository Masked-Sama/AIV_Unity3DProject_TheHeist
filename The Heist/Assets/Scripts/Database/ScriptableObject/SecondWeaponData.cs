using UnityEngine;
[CreateAssetMenu(fileName = "SecondWeaponDataTemplate",    menuName = "WeaponsData/SecondaryWeapons", order = 1)]
public class SecondWeaponData : WeaponData
{
    public void Awake()
    {
        itemType = ItemType.SecondWeapon;
    }
}