using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class GrenadeThrow : PlayerAbilityBase, IPoolRequester
    {

        [Header("Variables References")]
        [SerializeField]
        private Transform attackPoint;
        [SerializeField]
        private PoolData[] grenadeType;

        [Header("Settings")]
        [SerializeField] private float throwCooldown;

        [Header("Throwing")]
        [SerializeField] private float throwForce;
        [SerializeField] private float throwUpwardForce;

        private GrenadeType currentGrenadeType;
        private bool canThrow = true;
        private float currentThrowCD = 0;
        private int currentAmount;

        public PoolData[] Datas
        {
            get { return grenadeType; }
        }


        public void OnEnable()
        {
            InputManager.Player.ThrowGrenade.performed += Throw;
            currentGrenadeType = GrenadeType.Incendiary;
            
        }

        public void OnDisable()
        {
            InputManager.Player.ThrowGrenade.performed -= Throw;
        }

        public override void OnInputDisabled()
        {
            isPrevented = true;
        }

        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility()
        {

        }

        private void Throw(InputAction.CallbackContext e)
        {
            if (!CanThrow()) return;
            //Player mi passa un valore per determinare la granata

            //Scelgo la pool in base al valore
            IGrenade grenadeComponent;
            switch (currentGrenadeType)
            {
                case GrenadeType.Incendiary:
                grenadeComponent = Pooler.Istance.GetPooledObject(grenadeType[0]).GetComponent<IGrenade>();
                break;
                case GrenadeType.Stun:
                grenadeComponent = Pooler.Istance.GetPooledObject(grenadeType[1]).GetComponent<IGrenade>();
                break;
                default: return;
            }
            //Get From Pool
            if (grenadeComponent == null)
            {
                return;
            }

            grenadeComponent.Throw(playerController.CameraPositionTransform, attackPoint, throwForce, throwUpwardForce);
            playerController.OnThrowGrenade?.Invoke(grenadeComponent);
            currentAmount--;
            playerController.Inventory.InventorySlots[(int)ItemType.ThrowableWeapon].AddAmount(-1);
            canThrow = false;
            currentThrowCD = throwCooldown;
            GameObject currentGrenadePrefab = playerController.Inventory.InventorySlots[(int)ItemType.ThrowableWeapon].ItemData.Prefab;
            GlobalEventManager.CastEvent(GlobalEventIndex.Shoot, GlobalEventArgsFactory.ShootFactory(currentGrenadePrefab, 1));

        }

        private void Update()
        {
            //Da cambiare perchè non ha senso qui
            currentAmount = playerController.Inventory.InventorySlots[(int)ItemType.ThrowableWeapon].Amount;
            if (currentThrowCD > 0)
            {
                currentThrowCD -= Time.deltaTime;
                if (currentThrowCD <= 0)
                {
                    canThrow = true;
                }
            }
        }

        private bool CanThrow()
        {
            return !isPrevented && canThrow && currentAmount>0;
        }
    }
}