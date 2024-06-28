using UnityEngine;

namespace Player
{
    public class PlayerCurrency : PlayerAbilityBase
    {
        [SerializeField]
        private int money; // Imposta un valore iniziale per la valuta del giocatore

        private bool canSpend;
        private bool canAdd;

        public int Money
        {
            get { return money; }
        }

        private void OnEnable()
        {
            playerController.OnTryToBuyItem += OnTryToBuyItem;
            
        }

        private void OnDisable()
        {
            playerController.OnTryToBuyItem -= OnTryToBuyItem;
        }

        private bool OnTryToBuyItem(int cost)
        {
            return SpendMoney(cost);
        }


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
            Debug.Log("Not enough money. Current balance: " + money);
            return false;            
        }

        // Metodo per verificare se il giocatore pu� permettersi un certo costo
        public bool CanAfford(int cost)
        {
            return money >= cost;
        }

        #region PrivateMethods
        private bool CanSpend()
        {
            return canSpend;
        }

        private bool CanAdd()
        {
            return canAdd;
        }
        #endregion

        #region PublicMethods
        public override void OnInputDisabled()
        {
            isPrevented = true;
        }

        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility() { }
        #endregion
    }
}
