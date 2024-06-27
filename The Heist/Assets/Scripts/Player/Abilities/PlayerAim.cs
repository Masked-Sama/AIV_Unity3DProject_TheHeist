using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAim : PlayerAbilityBase
    {
        private const string aimStringParameter = "Aim";

        [SerializeField]
        private CinemachineVirtualCamera vCam;
        [SerializeField]
        private float zoomInValue;

        private void OnEnable()
        {
            InputManager.Player.Aim.performed += OnAimPerformed;
            InputManager.Player.Aim.canceled += OnAimCanceled;

            // playerController.OnChangeWeapon += OnChangeWeapon; // Disable
            playerController.OnGroundLanded += OnGroundLanded; // Enable
            playerController.OnGroundReleased += OnGroundReleased; // Disable
            playerController.OnRunEnded += OnRunEnded; // Enable
            playerController.OnRunStarted += OnRunStarted; // Disable
            // playerController.OnThrowGrenade += OnThrowGrenade; // Disable
        }

        private void OnDisable()
        {
            InputManager.Player.Aim.performed -= OnAimPerformed;
            InputManager.Player.Aim.canceled -= OnAimCanceled;

            //playerController.OnChangeWeapon -= OnChangeWeapon; // Disable
            playerController.OnGroundLanded -= OnGroundLanded; // Enable
            playerController.OnGroundReleased -= OnGroundReleased; // Disable
            playerController.OnRunEnded -= OnRunEnded; // Enable
            playerController.OnRunStarted -= OnRunStarted; // Disable
            //playerController.OnThrowGrenade -= OnThrowGrenade; // Disable
        }

        private void OnThrowGrenade(IGrenade grenade)
        {
            SetAimEnable(false);
        }

        private void OnRunStarted()
        {
            SetAimEnable(false);
        }

        private void OnRunEnded()
        {
            SetAimEnable(true);
        }

        private void OnGroundReleased()
        {
            SetAimEnable(false);
        }

        private void OnGroundLanded()
        {
            SetAimEnable(true);
        }

        private void OnChangeWeapon(WeaponData data)
        {
            SetAimEnable(false);
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
            SetAimEnable(false);
        }

        private bool CanAim()
        {
            return !isPrevented;
        }

        private void SetAimEnable(bool value)
        {
            isPrevented = !value;
        }

    }
}
