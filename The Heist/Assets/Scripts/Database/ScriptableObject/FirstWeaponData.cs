using UnityEngine;

[CreateAssetMenu(fileName = "FirstWeaponDataTemplate", menuName = "WeaponsData/FirstWeapons", order = 1)]
public class FirstWeaponData: WeaponData
{
    public void Awake()
    {
        itemType = ItemType.FirstWeapon;
    }
}