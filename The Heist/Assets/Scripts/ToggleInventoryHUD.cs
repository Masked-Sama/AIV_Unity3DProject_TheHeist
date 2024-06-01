using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ToggleInventoryHUD : MonoBehaviour
{
    InventoryUI inventoryGUI;

    [SerializeField]
    InventoryObject inventoryObject;

    
    private UnityAction<GlobalEventArgs> OnAddToInventory;

    private void Awake()
    {
        inventoryGUI = GetComponent<UIDocument>().rootVisualElement.Q<InventoryUI>("InventoryUI"); 
        
    }
    private void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.AddItemToInventory, OnAddToInventory);
        OnAddToInventory += CreateInventory;
    }



    //private void Update()   //DA CAMBIARE APPENA C'è IL GLOBAL EVENT MANAGER
    //{
    //    CreateInventory(); 
    //}



    private void CreateInventory(GlobalEventArgs message)
    {

        GlobalEventArgsFactory.AddItemToInventoryParser(message, out GameObject itemToAdd);
        Item component = itemToAdd.GetComponent<Item>();
        if (component == null) return;
        Debug.Log("INVENTARIO: " + itemToAdd.gameObject.name);
    }
}
