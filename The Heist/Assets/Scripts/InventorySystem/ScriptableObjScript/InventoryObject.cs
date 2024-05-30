using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory", menuName = "Inventory System/Inventories")]
public class InventoryObject: ScriptableObject
{
    [SerializeField]
    private List<InventorySlot> inventoryObjects;

    public int ItemsNumber { get {  return inventoryObjects.Count; } }
    public List<InventorySlot> InventorySlot { get {  return inventoryObjects; } }

    private void Awake()
    {
        inventoryObjects = new List<InventorySlot>();
    }
    public void AddItem(ItemObject item, int amount)
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
}