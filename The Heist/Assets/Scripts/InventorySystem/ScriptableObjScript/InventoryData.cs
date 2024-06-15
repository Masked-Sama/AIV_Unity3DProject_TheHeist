using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory", menuName = "Inventory System/Inventories")]
public class InventoryData: ScriptableObject
{
    [SerializeField]
    private List<InventorySlot> inventoryObjects;

    public int SlotsNumber { get {  return inventoryObjects.Count; } }
    public List<InventorySlot> InventorySlots { get {  return inventoryObjects; } }

    private void Awake()
    {
        inventoryObjects = new List<InventorySlot>();
    }
    
    public void AddItem(ItemData item, int amount, bool isPlayerInventory = false) 
    {
        for (int i = 0; i < inventoryObjects.Count; i++)
        {
            if (inventoryObjects[i].ItemData == item)
            {
                inventoryObjects[i].AddAmount(amount);
                return;
            }
        }
        if (!isPlayerInventory) 
        {
            inventoryObjects.Add(new InventorySlot(item, amount));
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
        inventoryObjects[index] = new InventorySlot(item,amount);
    }

}