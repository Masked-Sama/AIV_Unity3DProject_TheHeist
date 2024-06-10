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
        private float runSpeed;
        [SerializeField]
        private float rotationSpeed;
        [SerializeField]
        private Transform playerMesh;

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

        private void Start()
        {
            Quaternion startRotation = Quaternion.Euler(new Vector3(
                playerController.CameraPositionTransform.forward.x,
                0,
                playerController.CameraPositionTransform.forward.z));
            playerMesh.rotation = startRotation;
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
            Vector3 forwardDirection = playerController.CameraPositionTransform.forward * playerController.ComputedDirection.y;
            Vector3 rightDirection = playerController.CameraPositionTransform.right * playerController.ComputedDirection.x;
            Vector3 finalDirection = (forwardDirection + rightDirection).normalized;
            computedSpeed.x = finalDirection.x;
            computedSpeed.y = finalDirection.z;
            computedSpeed *= currentSpeed;
            SetSpeed(computedSpeed);

            PlayerLookTowardsInput(finalDirection.normalized);
        }

        private void PlayerLookTowardsInput(Vector3 finalDirection)
        {
            // Rotation depending by movement.
            Quaternion targetRotation = Quaternion.Euler(0, playerController.CameraPositionTransform.eulerAngles.y, 0);
            playerMesh.rotation = Quaternion.Slerp(playerMesh.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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