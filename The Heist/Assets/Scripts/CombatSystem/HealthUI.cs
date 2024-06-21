using UnityEngine.UIElements;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;

public class HealthUI : VisualElement
{
    public new class UXmlFactory : UxmlFactory<HealthUI, UxmlTraits>
    {
    }

    private VisualTreeAsset healthBarTemplate;
    private VisualElement healthBarContainer;

    private float currentHealth;
    private float maxHealth;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { 
            currentHealth = value;
            SetHealthUI();
            }
    }

    public HealthUI()
    {
        healthBarTemplate = Resources.Load<VisualTreeAsset>("HealthBar");
        healthBarContainer = new VisualElement();
        healthBarContainer.name = "Health";
        hierarchy.Add(healthBarContainer);
    }

    private void SetHealthUI()
    {
        healthBarContainer.Clear();
        var hb = healthBarTemplate.Instantiate();
        SetProgressBarParam(hb.Q<ProgressBar>("HealthBar"), currentHealth);
        healthBarContainer.Add(hb);
    }

    private void SetProgressBarParam(ProgressBar progressBar, float value)
    {
        float percentValue = (value / maxHealth) * 100;
        progressBar.value = percentValue;
    }

}
