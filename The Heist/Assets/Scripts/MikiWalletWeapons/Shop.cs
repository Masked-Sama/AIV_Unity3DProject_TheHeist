using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    
    public GameObject shopUI;
    public PlayerCurrency playerCurrency;
    public int weaponCost = 50;

    private bool playerInRange = false;

    void Update()
    {

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }

    public void BuyWeapon()
    {

        if (playerCurrency.CanAfford(weaponCost))
        {
            playerCurrency.SpendCurrency(weaponCost);
            Debug.Log("Arma acquistata");

        }
        else
        {
            Debug.Log("Non hai i cash");
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Sei vicino alllo shop. Premi 'E' per interagire");
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
