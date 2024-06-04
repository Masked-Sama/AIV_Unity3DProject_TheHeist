using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory", menuName = "Inventory System/Inventories")]
public class InventoryObject: ScriptableObject
{
    [SerializeField]
    private List<InventorySlot> inventoryObjects;

    public int SlotsNumber { get {  return inventoryObjects.Count; } }
    public List<InventorySlot> InventorySlots { get {  return inventoryObjects; } }

    private void Awake()
    {
        inventoryObjects = new List<InventorySlot>();
    }
    
    private void AddItem(ItemObject item, int amount) 
    {
        for (int i = 0; i < inventoryObjects.Count; i++)
        {
            if (inventoryObjects[i].Item == item)
            {
                inventoryObjects[i].AddAmount(amount);
                return;
            }
        }
        inventoryObjects.Add(new InventorySlot(item,amount));
    }

    public void ChangeItem(ItemObject item, int amount, int index)
    {
        if (SlotsNumber <= index || amount <= 0) return;
        inventoryObjects[index] = new InventorySlot(item,amount);
    }

}