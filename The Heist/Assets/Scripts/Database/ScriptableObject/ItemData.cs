using UnityEngine;

public enum ItemType
{
    FirstWeapon,
    SecondWeapon,
    ThrowableWeapon,
    Consumable
}

public interface IInventoried 
{
    Texture2D Texture2D { get;}
}

public abstract class ItemData : ScriptableObject
{
    [SerializeField]
    protected string itemName;
    [SerializeField]
    protected GameObject prefab;
    [SerializeField]
    protected int cost;

    protected ItemType itemType;

    public string ItemName {  get { return itemName; } }
    public GameObject Prefab {  get { return prefab; } }
    public ItemType ItemType { get { return itemType; } }
    public int Cost { get { return cost; } }
}
