using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerInventoryHUD : MonoBehaviour
{
    private InventoryUI inventoryGUI;
    [SerializeField]
    private InventoryObject inventoryObject;

    private void Awake()
    {
        inventoryGUI = GetComponent<UIDocument>().rootVisualElement.Q<InventoryUI>("InventoryUI");
        inventoryGUI.MaxSlotsNumber = 3;
    }
    private void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.AddItemToInventory, UpdateInventory);
    }
    private void UpdateInventory(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.AddItemToInventoryParser(message, out GameObject itemToAdd);
        Item item = itemToAdd.GetComponent<Item>();

        switch(item.ItemObj.Type)
        {
            case ItemType.Ammunition:
                inventoryObject.ChangeItem(item.ItemObj, 1, 1);
                inventoryGUI.SwitchSlotItem(1, item);
                    break;

            case ItemType.Weapon:
                inventoryObject.ChangeItem(item.ItemObj, 1, 0);
                inventoryGUI.SwitchSlotItem(0, item);
                break;

            case ItemType.Consumable:
                inventoryObject.ChangeItem(item.ItemObj, 1, 2);
                inventoryGUI.SwitchSlotItem(2, item);
                break;
        }
    }
}
