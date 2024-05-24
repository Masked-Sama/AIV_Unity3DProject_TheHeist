using System;
using UnityEngine;

namespace Player
{
    public class PlayerJump : PlayerAbilityBase
    {
        [SerializeField]
        private float jumpCooldown;

        private float lastJumpTime;

        private void OnEnable()
        {
            InputManager.Player.Jump.performed += OnJumpPerformed;
        }

        private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!CanJump()) return;
            // Do jump.
            Debug.Log("Jump");
        }

        private void OnGroundLanded()
        {
            isPrevented = false;
        }

        private void OnGroundReleased()
        {
            isPrevented = true;
        }

        private void OnDisable()
        {
            InputManager.Player.Jump.performed -= OnJumpPerformed;
        }

        private bool CanJump()
        {
            return !isPrevented 
                //&& (Time.time - lastJumpTime) >= jumpCooldown
                && playerController.IsGrounded;
        }

        public override void OnInputDisabled() { }

        public override void OnInputEnabled() { }

        public override void StopAbility() { }
    }
}
