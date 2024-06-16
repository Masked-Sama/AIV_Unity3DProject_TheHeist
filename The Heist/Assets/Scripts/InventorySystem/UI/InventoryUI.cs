using UnityEngine.UIElements;
using UnityEngine;
using System;
using UnityEditor.Graphs;

public class InventoryUI : VisualElement
{
    //Factory class per definire nuovi elementi
    public new class UXmlFactory : UxmlFactory<InventoryUI, UxmlTraits>
    {
    }

    //aggiungiamo un nuovo elemento nell'inspector dell UI toolkit che andranno a settare i valori  degli inventories
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription maxSlotsNumber = new UxmlIntAttributeDescription()  //nuova variabile intera Max_Slots_Size
        {
            name = "Max_Slots_Number"
        };
       
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)    //linko i valori che andrò a inserire con i valori della classe PointBar
        {
            base.Init(ve, bag, cc);

            (ve as InventoryUI).MaxSlotsNumber = maxSlotsNumber.GetValueFromBag(bag, cc); 
        }
    }


    private VisualTreeAsset slotTemplate;
    private VisualElement slotContainer;
    private int maxSlotsNumber;
    public int MaxSlotsNumber { get { return maxSlotsNumber; } set { maxSlotsNumber = value; CreateSlots(); } }

    public InventoryUI()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("InventoryStyleSheet"));
        slotTemplate = Resources.Load<VisualTreeAsset>("InventorySlot");
        slotContainer = new VisualElement();
        slotContainer.name = "Inventory";
        slotContainer.AddToClassList("container");

        hierarchy.Add(slotContainer);
    }


    public void AddToSlotItem(int index, IInventoried itemObjToSwitch, int amount)
    {
        VisualElement slot =  slotContainer[index];
        if (slot == null) return;
        //condizione brutta per dire che hai già l'arma raccolta nell'inventario
        if (slot.Q<VisualElement>("ItemIcon").style.backgroundImage == itemObjToSwitch.Texture2D)
        {
            int newAmount = Math.Clamp(int.Parse(slot.Q<Label>("BulletsNumber").text) + amount,0,999) ;
            SetBulletsNumber(slotContainer[index], newAmount);
        }
        else
        {
            SwitchSlotItem(index, itemObjToSwitch, amount);
        }
    }


    #region Private Methods
    private void SwitchSlotItem(int index, IInventoried itemObjToSwitch, int amount)
    {
        SetBackgroundTexture(slotContainer[index], itemObjToSwitch.Texture2D);
        SetBulletsNumber(slotContainer[index], amount);
    }
    private void CreateSlots()
    {
        slotContainer.Clear();
        for (int i = 0; i < maxSlotsNumber; i++)
        {
            var slot = slotTemplate.Instantiate();
            slot.name = "Slot_" + i;
            slot.style.flexDirection = FlexDirection.Row;
            slotContainer.Add(slot);
        }
    }

    private void SetBackgroundTexture(VisualElement slot, Texture2D texture)
    {   
        slot.Q<VisualElement>("ItemIcon").style.backgroundImage = texture;
    }
    private void SetBulletsNumber(VisualElement slot, int number)
    {
        slot.Q<Label>("BulletsNumber").text = number.ToString();
    }
    #endregion

}
