using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public int money = 100; // Imposta un valore iniziale per la valuta del giocatore

    private void Start()
    {
        Debug.Log("Player money: " + money);
    }

    // Metodo per aggiungere denaro al giocatore
    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Added money: " + amount + ". New balance: " + money);
    }

    // Metodo per rimuovere denaro dal giocatore
    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            Debug.Log("Spent money: " + amount + ". New balance: " + money);
            return true;
        }
        else
        {
            Debug.Log("Not enough money. Current balance: " + money);
            return false;
        }
    }

    // Metodo per verificare se il giocatore può permettersi un certo costo
    public bool CanAfford(int cost)
    {
        return money >= cost;
    }
}
