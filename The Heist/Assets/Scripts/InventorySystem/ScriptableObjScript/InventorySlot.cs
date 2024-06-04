using System;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public enum SlotType
{
    FirstWeapon = 0, 
    SecondWeapon = 1,
    ThrowableWeapon = 2
}

[System.Serializable]
public class InventorySlot
{
    [SerializeField]
    private ItemObject item;
    [SerializeField]
    private int amount;

    public ItemObject Item { get { return item; } }
    public int Amount {  get { return amount; } }

    public InventorySlot(ItemObject item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

}
