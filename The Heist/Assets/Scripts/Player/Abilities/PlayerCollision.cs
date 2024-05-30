using UnityEngine;

namespace Player
{
    public class PlayerCollision : PlayerAbilityBase
    {
        [SerializeField]
        protected LayerMask groundLayer;
        [SerializeField]
        private SphereCollider groundCollider;

        private void Update()
        {
            DetectGroundCollision();
        }

        protected void DetectGroundCollision()
        {
            bool wasGrounded = playerController.IsGrounded;
            Vector3 centerPoint = groundCollider.transform.position;
            float sphereRadius = groundCollider.radius;
            playerController.IsGrounded = Physics.OverlapSphere(centerPoint, sphereRadius, groundLayer).Length != 0;
            if (wasGrounded == playerController.IsGrounded) return;
            if (wasGrounded)
            {
                playerController.OnGroundReleased?.Invoke();
            }
            else
            {
                playerController.OnGroundLanded?.Invoke();
            }
        }

        public override void OnInputDisabled()
        {
            isPrevented = true;
        }

        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility() { }
    }
}
