using UnityEngine;

public enum ItemType
{
    FirstWeapon,
    SecondWeapon,
    ThrowableWeapon,
    Consumable
}

public abstract class ItemData : ScriptableObject
{
    [SerializeField]
    protected GameObject prefab;
    [SerializeField]
    protected ItemType itemType;

    public GameObject Prefab {  get { return prefab; } }
    public ItemType ItemType { get { return itemType; } }

}
