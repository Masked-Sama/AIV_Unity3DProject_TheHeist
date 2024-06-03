using UnityEngine;

namespace Player
{
    public class PlayerLook : PlayerAbilityBase
    {
        // To polish.

        [SerializeField]
        private Transform thirdPersonCamera;
        [SerializeField]
        private float rotationSpeed;

        private Vector3 cameraPosition;

        public override void OnInputDisabled()
        {
            isPrevented = true;
        }

        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility()
        {

        }

        // Temp
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (!CanLook()) return;
            // Robe
        }

        private bool CanLook()
        {
            return !isPrevented;
        }
    }
}
