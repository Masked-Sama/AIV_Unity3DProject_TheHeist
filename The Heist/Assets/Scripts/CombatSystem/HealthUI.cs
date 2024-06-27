using UnityEngine.UIElements;
using UnityEngine;

public class HealthUI : VisualElement
{
    public new class UXmlFactory : UxmlFactory<HealthUI, UxmlTraits>
    {
    }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlFloatAttributeDescription currentHealth = new UxmlFloatAttributeDescription()  //nuova variabile intera Max_Slots_Size
        {
            name = "Current_Health"
        };
        UxmlFloatAttributeDescription maxHealth = new UxmlFloatAttributeDescription()  //nuova variabile intera Max_Slots_Size
        {
            name = "Max_Health"
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)    //linko i valori che andro a inserire con i valori della classe PointBar
        {
            base.Init(ve, bag, cc);

            (ve as HealthUI).CurrentHealth = currentHealth.GetValueFromBag(bag, cc);
            (ve as HealthUI).MaxHealth = maxHealth.GetValueFromBag(bag, cc);
        }
    }
    private VisualTreeAsset healthBarTemplate;
    private VisualElement healthBarContainer;

    private float currentHealth;
    private float maxHealth;

    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; SetHealthUI();} }
    public float MaxHealth { get { return currentHealth; } set { maxHealth = value; } }

    public HealthUI()
    {
        healthBarTemplate = Resources.Load<VisualTreeAsset>("HealthBar");
        healthBarContainer = new VisualElement();
        healthBarContainer.name = "Health";
        healthBarContainer.AddToClassList("healthBar");
        hierarchy.Add(healthBarContainer);
    }

    private void SetHealthUI()
    {
        healthBarContainer.Clear();
        var hb = healthBarTemplate.Instantiate();
        hb.name = "HealthBarTemplate";
        hb.AddToClassList("healthBar");
        SetProgressBarParam(hb.Q<VisualElement>("CurrentHealth"), currentHealth);
        healthBarContainer.Add(hb);
    }

    private void SetProgressBarParam(VisualElement progressBar, float value)
    {
        float percentValue = (value / maxHealth)*100;
        progressBar.style.width = Length.Percent(percentValue);
    }

}
