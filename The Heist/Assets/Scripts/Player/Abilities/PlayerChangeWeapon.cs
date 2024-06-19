using UnityEngine.InputSystem;
using UnityEngine;

namespace Player
{
    public class PlayerChangeWeapon : PlayerAbilityBase
    {
        #region Mono
        private void OnEnable()
        {
            InputManager.Player.ChangeWeapon.performed += OnChangeWeaponPerformed;
        }

        private void OnDisable()
        {
            InputManager.Player.ChangeWeapon.performed -= OnChangeWeaponPerformed;
        }
        #endregion

        private void OnChangeWeaponPerformed(InputAction.CallbackContext context)
        {
            if (!CanChangeWeapon()) return;
            WeaponData weapon;
            switch (int.Parse(context.control.displayName))
            {
                case 1:
                    weapon = (WeaponData)playerController.Inventory.GetItem((int)ItemType.FirstWeapon);                    
                    break;
                case 2:
                    weapon = (WeaponData)playerController.Inventory.GetItem((int)ItemType.SecondWeapon);                    
                    break;
                default: return;
            }
            if (weapon == null) return;
            playerController.OnChangeWeapon?.Invoke(weapon);
        }
        
        #region PrivateMethods
        private bool CanChangeWeapon()
        {
            return !isPrevented;
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
