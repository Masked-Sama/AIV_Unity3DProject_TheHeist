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
    private ItemObject itemObj;
    [SerializeField]
    private int amount;

    public ItemObject ItemObj { get { return itemObj; } }
    public int Amount {  get { return amount; } }

    public InventorySlot(ItemObject item, int amount)
    {
        this.itemObj = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

}
