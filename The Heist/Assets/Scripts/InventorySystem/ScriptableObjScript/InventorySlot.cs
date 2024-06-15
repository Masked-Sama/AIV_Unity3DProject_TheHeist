using UnityEngine;

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
    private ItemData itemData;
    [SerializeField]
    private int amount;

    public ItemData ItemData { get { return itemData; } }
    public int Amount {  get { return amount; } }

    public InventorySlot(ItemData item, int amount)
    {
        this.itemData = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

}
