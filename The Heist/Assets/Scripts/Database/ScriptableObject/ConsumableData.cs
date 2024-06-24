using UnityEngine;
[CreateAssetMenu(fileName = "ConsumableDataTemplate",  menuName = "ItemData", order = 1)]
public class ConsumableData : ItemData
{
    [SerializeField] private float hp;
    public float HP { get { return hp; } }

    private void Awake()
    {
        itemType = ItemType.Consumable;
    }
}