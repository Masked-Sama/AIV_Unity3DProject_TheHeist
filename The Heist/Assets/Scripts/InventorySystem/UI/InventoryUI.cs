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
        UxmlIntAttributeDescription maxSlotsSize = new UxmlIntAttributeDescription()  //nuova variabile intera Max_Slots_Size
        {
            name = "Max_Slots_Size"
        };
       
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)    //linko i valori che andrò a inserire con i valori della classe PointBar
        {
            base.Init(ve, bag, cc);

            (ve as InventoryUI).MaxSlotsSize = maxSlotsSize.GetValueFromBag(bag, cc); 
        }
    }


    private int maxSlotsSize;
    public int MaxSlotsSize { get { return maxSlotsSize; } set { maxSlotsSize = value; CreateSlots(); } }

    private VisualTreeAsset slotTemplate;
    private VisualElement slotContainer;
    private VisualElement[] slots;

    public InventoryUI()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("InventoryStyleSheet"));
        slotTemplate = Resources.Load<VisualTreeAsset>("InventorySlot");
        slotContainer = new VisualElement();
        slotContainer.name = "Inventory";
        slotContainer.AddToClassList("container");

        hierarchy.Add(slotContainer);
    }

    private void CreateSlots()
    {
        slotContainer.Clear();
        slots = new VisualElement[maxSlotsSize];
        for (int i = 0; i < maxSlotsSize; i++)
        {
            var slot = slotTemplate.Instantiate();
            slot.name = "Slot_" + i;
            slot.style.flexDirection = FlexDirection.Row;
            slotContainer.Add(slot);
            slots[i] = slot;
        }
    }

}
