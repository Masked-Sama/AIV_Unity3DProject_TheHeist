using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAim : PlayerAbilityBase
    {
        private const string aimStringParameter = "Aim";

        #region SerializeField
        [SerializeField]
        private CinemachineVirtualCamera vCam;
        [SerializeField]
        private float zoomInValue;
        #endregion

        #region Mono
        private void OnEnable()
        {
            InputManager.Player.Aim.performed += OnAimPerformed;
            InputManager.Player.Aim.canceled += OnAimCanceled;

            playerController.OnGroundLanded += OnGroundLanded; // Enable
            playerController.OnGroundReleased += OnGroundReleased; // Disable
            playerController.OnRunEnded += OnRunEnded; // Enable
            playerController.OnRunStarted += OnRunStarted; // Disable
        }

        private void OnDisable()
        {
            InputManager.Player.Aim.performed -= OnAimPerformed;
            InputManager.Player.Aim.canceled -= OnAimCanceled;

            playerController.OnGroundLanded -= OnGroundLanded; // Enable
            playerController.OnGroundReleased -= OnGroundReleased; // Disable
            playerController.OnRunEnded -= OnRunEnded; // Enable
            playerController.OnRunStarted -= OnRunStarted; // Disable
        }
        #endregion

        #region PrivateMethods
        private bool CanAim()
        {
            return !isPrevented;
        }

        private void SetAimEnable(bool value)
        {
            isPrevented = !value;
        }

        private void OnThrowGrenade(IGrenade grenade)
        {
            StopAbility();
        }

        private void OnRunStarted()
        {
            StopAbility();
        }

        private void OnRunEnded()
        {
            SetAimEnable(true);
        }

        private void OnGroundReleased()
        {
            StopAbility();
        }

        private void OnGroundLanded()
        {
            SetAimEnable(true);
        }

        private void OnChangeWeapon(WeaponData data)
        {
            StopAbility();
        }

        private void OnAimPerformed(InputAction.CallbackContext context)
        {
            if (!CanAim()) return;
            playerController.IsAiming = true;
            playerVisual.SetAnimatorParameter(aimStringParameter, playerController.IsAiming);
            vCam.m_Lens.FieldOfView = zoomInValue;
        }

        private void OnAimCanceled(InputAction.CallbackContext context)
        {
            playerController.IsAiming = false;
            playerVisual.SetAnimatorParameter(aimStringParameter, playerController.IsAiming);
            vCam.m_Lens.FieldOfView = 45;
        }
        #endregion

        #region PublicMethods
        public override void OnInputDisabled()
        {
            SetAimEnable(true);
        }

        public override void OnInputEnabled()
        {
            SetAimEnable(false);
        }

        public override void StopAbility()
        {
            playerController.IsAiming = false;
            playerVisual.SetAnimatorParameter(aimStringParameter, playerController.IsAiming);
            vCam.m_Lens.FieldOfView = 45;

            SetAimEnable(false);
        }
        #endregion
    }
}
