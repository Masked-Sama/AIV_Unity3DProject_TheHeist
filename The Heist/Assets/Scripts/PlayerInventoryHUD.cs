using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventoryHUD : MonoBehaviour
{
    private const int maxSlotsNumber = 3;

    private InventoryUI inventoryGUI;

    private void Awake()
    {
        inventoryGUI = GetComponent<UIDocument>().rootVisualElement.Q<InventoryUI>("InventoryUI");
        inventoryGUI.MaxSlotsNumber = maxSlotsNumber;
    }
    private void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.AddItemToInventory, UpdateInventory);
    }

    private void UpdateInventory(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.AddItemToInventoryParser(message, out GameObject itemToAdd);
        Item item = itemToAdd.GetComponent<Item>();

        switch(item.ItemObj.ItemType)
        {
            case ItemType.SecondWeapon:
                inventoryGUI.SwitchSlotItem((int)SlotType.SecondWeapon, item);
                    break;

            case ItemType.FirstWeapon:
                inventoryGUI.SwitchSlotItem((int)SlotType.FirstWeapon, item);
                break;

            case ItemType.ThrowableWeapon:
                inventoryGUI.SwitchSlotItem((int)SlotType.ThrowableWeapon, item);
                break;
            default:
                return;
        }
    }
}
