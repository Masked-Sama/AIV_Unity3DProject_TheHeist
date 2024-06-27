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

        private GrenadeType currentGrenadeType = GrenadeType.Incendiary;
        private bool canThrow = true;
        private float currentThrowCD = 0;

        #region Interface: IPoolRequest
        public PoolData[] Datas
        {
            get { return grenadeType; }
        }
        #endregion

        #region Mono
        public void OnEnable()
        {
            InputManager.Player.ThrowGrenade.performed += Throw;
            playerController.OnPickUpItem += PickUpGrenade;
        }

        public void OnDisable()
        {
            InputManager.Player.ThrowGrenade.performed -= Throw;
            playerController.OnPickUpItem -= PickUpGrenade;
        }
        private void Update()
        {
            if (currentThrowCD > 0)
            {
                currentThrowCD -= Time.deltaTime;
                if (currentThrowCD <= 0)
                {
                    canThrow = true;
                }
            }
        }
        #endregion

        #region Override
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
        #endregion

        #region Private Methods
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
            if (grenadeComponent == null) return;

            grenadeComponent.Throw(playerController.CameraPositionTransform, attackPoint, throwForce, throwUpwardForce);
            playerController.OnThrowGrenade?.Invoke(grenadeComponent);
            playerController.Inventory.InventorySlots[(int)ItemType.ThrowableWeapon].AddAmount(-1);
            canThrow = false;
            currentThrowCD = throwCooldown;

            GameObject currentGrenadePrefab = playerController.Inventory.InventorySlots[(int)ItemType.ThrowableWeapon].ItemData.Prefab;
            GlobalEventManager.CastEvent(GlobalEventIndex.Shoot, GlobalEventArgsFactory.ShootFactory(currentGrenadePrefab, 1));

        }
        private bool CanThrow()
        {
            return !isPrevented
                && canThrow
                && playerController.Inventory.InventorySlots[(int)ItemType.ThrowableWeapon].Amount > 0;
        }
        private void PickUpGrenade(ItemData item) 
        {
            if (!(item is ThrowableData)) return;
            ThrowableData grenade = item as ThrowableData;
            currentGrenadeType = grenade.GrenadeType;
        }
        #endregion
    }
}