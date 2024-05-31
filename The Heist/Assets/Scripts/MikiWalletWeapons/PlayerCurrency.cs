using System.Diagnostics;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public int currency = 100;

    public bool CanAfford(int amount)
    {
        return currency >= amount;
    }

    public void SpendCurrency(int amount)
    {
        if (CanAfford(amount))
        {
            currency -= amount;
            
        }
    }
}