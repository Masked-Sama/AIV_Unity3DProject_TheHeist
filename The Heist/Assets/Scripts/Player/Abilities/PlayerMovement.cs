using PlasticGui.WorkspaceWindow.QueryViews.Changesets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : PlayerAbilityBase
    {
        private const float moveThreshold = 0.05f;

        [SerializeField]
        private float walkSpeed;
        [SerializeField]
        private float runSpeed;

        protected InputAction moveAction;
        protected bool wasWalking;
        protected float currentSpeed;

        #region Mono
        private void OnEnable()
        {
            moveAction = InputManager.Player.Move;
        }

        private void Update()
        {
            if (!CanMove()) return;
            FillDirectionFromInput();
            Move();
            //Turn();
            HandleEvents();
        }
        #endregion

        #region PublicMethods
        public override void OnInputDisabled()
        {
            isPrevented = true;
            playerController.ComputedDirection = Vector2.zero;
            StopAbility();
        }

        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility()
        {
            SetSpeed(Vector2.zero);
        }
        #endregion

        #region InternalMethods
        private void SetSpeed(Vector2 speed)
        {
            Vector3 currentVelocity = playerController.GetVelocity();
            currentVelocity.x = speed.x;
            currentVelocity.z = speed.y;
            playerController.SetVelocity(currentVelocity);
        }

        private bool CanMove()
        {
            return !isPrevented;
        }

        protected void FillDirectionFromInput()
        {
            playerController.ComputedDirection = moveAction.ReadValue<Vector2>();
        }

        protected void Move()
        {
            //currentSpeed = wasWalking ? walkSpeed : runSpeed;
            currentSpeed = walkSpeed;
            Vector2 computedSpeed = playerController.ComputedDirection * currentSpeed;
            SetSpeed(computedSpeed);
        }

        protected void HandleEvents()
        {
            bool isWalking = currentSpeed > moveThreshold;
            if (!wasWalking && isWalking) playerController.OnWalkStarted?.Invoke();
            if(wasWalking && !isWalking) playerController.OnWalkEnded?.Invoke();
            wasWalking = isWalking;
        }
        #endregion
    }
}

