using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : PlayerAbilityBase
    {
        private const float moveThreshold = 0.05f;

        #region SerializeField
        [SerializeField]
        private float walkSpeed;
        [SerializeField]
        private Transform playerMesh;
        [SerializeField]
        private Transform cameraPivot; // Switch camera with this.
        [SerializeField]
        private float runSpeed;
        #endregion

        #region InternalMembers
        protected InputAction moveAction;
        protected InputAction runAction;

        protected Vector2 computedSpeed;

        protected bool wasWalking;
        protected bool wasRunning;

        protected bool runKeyPressed;
        #endregion

        #region Mono
        private void OnEnable()
        {
            moveAction = InputManager.Player.Move;
            runAction = InputManager.Player.Run;
            runAction.performed += OnRunActionPerformed;
            runAction.canceled += OnRunActionCanceled;
        }

        private void OnDisable()
        {
            runAction.performed -= OnRunActionPerformed;
            runAction.canceled -= OnRunActionCanceled;
        }

        private void Update()
        {
            if (!CanMove()) return;
            FillDirectionFromInput();
            Move();
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
            float currentSpeed = runKeyPressed ? runSpeed : walkSpeed;
            Vector3 forwardDirection = playerController.PlayerTransform.forward * playerController.ComputedDirection.y;
            Vector3 rightDirection = playerController.PlayerTransform.right * playerController.ComputedDirection.x;
            Vector3 finalDirection = (forwardDirection + rightDirection).normalized;
            computedSpeed.x = finalDirection.x;
            computedSpeed.y = finalDirection.z;
            computedSpeed *= currentSpeed;
            SetSpeed(computedSpeed);

            if (computedSpeed.sqrMagnitude <= moveThreshold * moveThreshold) return;

            // Rotation depending by movement.
            Vector3 directionToLook = (playerController.PlayerTransform.position - playerController.CameraPositionTransform.position).normalized;
            directionToLook.y = 0;
            playerController.PlayerTransform.forward = Vector3.Slerp(playerController.PlayerTransform.forward, directionToLook, Time.deltaTime * 10);

            playerMesh.forward = Vector3.Slerp(playerMesh.forward, finalDirection.normalized, Time.deltaTime * 10);
        }

        protected void HandleEvents()
        {
            Vector3 movementVelocity = playerController.GetVelocity();
            float lengthSquared = playerController.GetDistanceSquared(velocityX: movementVelocity.x, velocityZ: movementVelocity.z);

            bool isRunning = lengthSquared > walkSpeed * walkSpeed;
            bool isWalking = (lengthSquared > moveThreshold * moveThreshold) && !isRunning;

            if (isWalking && !wasWalking) playerController.OnWalkStarted?.Invoke();
            if (isRunning && !wasRunning) playerController.OnRunStarted?.Invoke();
            if (wasRunning && !isRunning) playerController.OnRunEnded?.Invoke();
            if (wasWalking && !isWalking) playerController.OnWalkEnded?.Invoke();

            wasWalking = isWalking;
            wasRunning = isRunning;
        }

        private void OnRunActionCanceled(InputAction.CallbackContext context)
        {
            SetRunKeyIsPressed(false);
        }

        private void OnRunActionPerformed(InputAction.CallbackContext context)
        {
            SetRunKeyIsPressed(true);
        }

        private void SetRunKeyIsPressed(bool value)
        {
            runKeyPressed = value;
        }
        #endregion
    }
}