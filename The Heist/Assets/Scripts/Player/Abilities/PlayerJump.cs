using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerJump : PlayerAbilityBase
    {
        [SerializeField]
        private float jumpForce;

        #region Mono
        private void OnEnable()
        {
            playerController.OnGroundLanded += OnGroundLanded;
            InputManager.Player.Jump.performed += OnJumpPerformed;
        }

        private void OnDisable()
        {
            playerController.OnGroundLanded -= OnGroundLanded;
            InputManager.Player.Jump.performed -= OnJumpPerformed;
        }
        #endregion

        #region PrivateMethods
        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            if (!CanJump()) return;
            playerController.SetVelocity(Vector3.up * jumpForce);
            playerController.IsJumping = true;
            playerController.JumpStarted?.Invoke();
        }

        private void OnGroundLanded()
        {
            playerController.IsJumping = false;
        }

        private bool CanJump()
        {
            return !isPrevented && playerController.IsGrounded;
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
