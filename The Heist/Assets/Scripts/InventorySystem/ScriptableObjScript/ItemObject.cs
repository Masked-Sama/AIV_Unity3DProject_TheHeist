using UnityEngine;

public enum ItemType
{
    Weapon,
    Ammunition,
    Consumable
}

public abstract class ItemObject : ScriptableObject
{
    [SerializeField]
    protected GameObject prefab;
    [SerializeField]
    protected string description;
    [SerializeField]
    protected ItemType type;
    [SerializeField]
    protected Texture2D texture;

    public GameObject Prefab {  get { return prefab; } }
    public string Description { get { return description; } }
    public ItemType Type { get { return type; } }
    public Texture2D Texture { get { return texture; } }

}
