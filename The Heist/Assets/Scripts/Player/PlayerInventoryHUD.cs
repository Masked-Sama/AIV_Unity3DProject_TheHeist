using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventoryHUD : MonoBehaviour
{
    private const int maxSlotsNumber = 3;
    private InventoryUI inventoryUI;
    private HealthUI healthUI;

    [SerializeField]
    private InventoryData playerInventory;


    private void Awake()
    {
        inventoryUI = GetComponent<UIDocument>().rootVisualElement.Q<InventoryUI>("InventoryUI");
        healthUI = GetComponent<UIDocument>().rootVisualElement.Q<HealthUI>("HealthUI");
        inventoryUI.MaxSlotsNumber = maxSlotsNumber;
        healthUI.CurrentHealth = 100.0f;
        Init();
    }
    private void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.AddItemToInventory, UpdateInventory);
        GlobalEventManager.AddListener(GlobalEventIndex.Shoot, ShootListener);
    }

    private void Init()
    {
        foreach(var item in playerInventory.InventorySlots)
        {
            if (item.ItemData == null) continue;
            inventoryUI.AddToSlotItem((int)item.ItemData.ItemType,(IInventoried)item.ItemData,item.Amount);
        }
    }

    private void UpdateInventory(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.AddItemToInventoryParser(message, out GameObject itemToAdd);
        Item item = itemToAdd.GetComponent<Item>();

        switch(item.ItemData.ItemType)
        {
            case ItemType.SecondWeapon:
                inventoryUI.AddToSlotItem((int)SlotType.SecondWeapon, (IInventoried)item.ItemData,item.Quantity);
                    break;

            case ItemType.FirstWeapon:
                inventoryUI.AddToSlotItem((int)SlotType.FirstWeapon, (IInventoried)item.ItemData, item.Quantity);
                break;

            case ItemType.ThrowableWeapon:
                inventoryUI.AddToSlotItem((int)SlotType.ThrowableWeapon, (IInventoried)item.ItemData, item.Quantity);
                break;
            default:
                return;
        }
    }
    private void ShootListener(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.ShootParser(message, out GameObject weapon,out int bulletAmount);
        ItemData weaponItemData = weapon.GetComponent<Item>().ItemData;
        IInventoried inventoried = (IInventoried)weaponItemData;
        if (inventoried == null) return;
        inventoryUI.AddToSlotItem((int)weaponItemData.ItemType, inventoried, bulletAmount*(-1));

    }
}
