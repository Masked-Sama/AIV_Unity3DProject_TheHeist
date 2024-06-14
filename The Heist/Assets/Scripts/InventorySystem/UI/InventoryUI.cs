using UnityEngine.UIElements;
using UnityEngine;

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


    public void SwitchSlotItem(int index, ItemData itemObjToSwitch, int amount)
    {
        if (slotContainer[index] == null) return;
        //SetBackgroundTexture(slotContainer[index], itemObjToSwitch.Texture);
        SetBulletsNumber(slotContainer[index], amount); 
    }

    #region Private Methods
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
