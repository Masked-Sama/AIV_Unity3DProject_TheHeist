using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventoryHUD : MonoBehaviour
{
    private const int maxSlotsNumber = 3;
    private InventoryUI inventoryGUI;

    [SerializeField]
    private InventoryObject playerInventory;

    private void Awake()
    {
        inventoryGUI = GetComponent<UIDocument>().rootVisualElement.Q<InventoryUI>("InventoryUI");
        inventoryGUI.MaxSlotsNumber = maxSlotsNumber;
        Init();
    }
    private void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.AddItemToInventory, UpdateInventory);
    }

    private void Init()
    {
        foreach(var item in playerInventory.InventorySlots)
        {

            inventoryGUI.SwitchSlotItem((int)item.ItemObj.ItemType,item.ItemObj,item.Amount);
        }
    }

    private void UpdateInventory(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.AddItemToInventoryParser(message, out GameObject itemToAdd);
        Item item = itemToAdd.GetComponent<Item>();

        switch(item.ItemData.ItemType)
        {
            case ItemType.SecondWeapon:
                inventoryGUI.SwitchSlotItem((int)SlotType.SecondWeapon, item.ItemData,item.Quantity);
                    break;

            case ItemType.FirstWeapon:
                inventoryGUI.SwitchSlotItem((int)SlotType.FirstWeapon, item.ItemData, item.Quantity);
                break;

            case ItemType.ThrowableWeapon:
                inventoryGUI.SwitchSlotItem((int)SlotType.ThrowableWeapon, item.ItemData, item.Quantity);
                break;
            default:
                return;
        }
    }
}
