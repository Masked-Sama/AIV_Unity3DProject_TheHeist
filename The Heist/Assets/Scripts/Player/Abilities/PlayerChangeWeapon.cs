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
            switch (int.Parse(context.control.displayName))
            {
                case 1:
                    WeaponData firstWeapon = (WeaponData)playerController.Inventory.GetItem(0);
                    if (firstWeapon == null) return;
                    playerController.OnChangeWeapon?.Invoke(firstWeapon);
                    Debug.Log("Hai cambiato in prima arma");
                    break;
                case 2:
                    WeaponData secondWeapon = (WeaponData)playerController.Inventory.GetItem(1);
                    if (secondWeapon == null) return;
                    playerController.OnChangeWeapon?.Invoke(secondWeapon);
                    Debug.Log("Hai cambiato in seconda arma");
                    break;
                default: return;
            }
            // Debug.Log(context.control.displayName);
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
