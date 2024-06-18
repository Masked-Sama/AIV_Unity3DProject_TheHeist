using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventories")]
public class InventoryData : ScriptableObject
{
    [SerializeField]
    private InventorySlot[] inventoryObjects;
    [SerializeField]
    private int maxSlotNumber;

    public int SlotsNumber { get { return maxSlotNumber; } }
    public InventorySlot[] InventorySlots { get { return inventoryObjects; } }

    private void Awake()
    {
        inventoryObjects = new InventorySlot[maxSlotNumber];
    }

    public void AddItem(ItemData item, int amount)
    {
        
        if (inventoryObjects[(int)item.ItemType].ItemData == item)
        {
            inventoryObjects[(int)item.ItemType].AddAmount(amount);
            return;
        }

        switch (item.ItemType)
        {
            case ItemType.FirstWeapon:
                ChangeItem(item, amount, ((int)SlotType.FirstWeapon));
                break;
            case ItemType.SecondWeapon:
                ChangeItem(item, amount, ((int)SlotType.SecondWeapon));
                break;
            case ItemType.ThrowableWeapon:
                ChangeItem(item, amount, ((int)SlotType.ThrowableWeapon));
                break;
            default:
                return;
        }

    }

    private void ChangeItem(ItemData item, int amount, int index)
    {
        if (SlotsNumber <= index || amount <= 0) return;
        inventoryObjects[index] = new InventorySlot(item, amount);
    }

    public ItemData GetItem(int index)
    {
        return inventoryObjects[index].ItemData;
    }

    public int FindWeaponSlot(ItemData item)
    {
        for (int i = 0; i < inventoryObjects.Length; i++)
        {
            if(inventoryObjects[i].ItemData == item)
                return i;
        }
        return -1;
    }

}