using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.UIElements;

public class ToggleInventoryHUD : MonoBehaviour
{
    InventoryUI inventoryGUI;

    [SerializeField]
    InventoryObject inventoryObject;

    private void Awake()
    {
        inventoryGUI = GetComponent<UIDocument>().rootVisualElement.Q<InventoryUI>("InventoryUI"); 
        inventoryGUI.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!inventoryGUI.visible) { CreateInventory(); }
            inventoryGUI.visible = !inventoryGUI.visible;

        }
    }

    private void CreateInventory()
    {
        int slotsNumber = inventoryObject.ItemsNumber;
        inventoryGUI.MaxSlotsSize = slotsNumber;
        for (int i = 0; i < slotsNumber; i++)
        {
            Texture2D texture = inventoryObject.InventorySlot[i].Item.Texture;
            VisualElement itemIcon = inventoryGUI.Q<VisualElement>("Slot_" + i).Q<VisualElement>("ItemIcon");
            itemIcon.style.backgroundImage = texture;
        }
    }
}
