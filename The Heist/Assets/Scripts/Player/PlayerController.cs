using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IDamageble
    {
        private const string currentSpeedParameter = "CurrentSpeed";

        #region SerializeField
        [SerializeField]
        protected Transform playerTransform;
        [SerializeField]
        protected Rigidbody playerRigidbody;
        [SerializeField]
        protected Collider playerPhysicsCollider;
        [SerializeField]
        protected PlayerVisual playerVisual;
        [SerializeField]
        protected Transform cameraPositionTransform;
        [SerializeField]
        protected HealthModule healthModule;
        #endregion

        private PlayerAbilityBase[] abilities;

        #region PlayerCollision
        public bool IsGrounded { get; set; }

        public Action OnGroundLanded;
        public Action OnGroundReleased;
        #endregion

        #region PlayerMovement
        public Vector2 ComputedDirection { get; set; }

        public Action OnWalkStarted;
        public Action OnWalkEnded;
        public Action OnRunStarted;
        public Action OnRunEnded;
        #endregion

        #region PublicProperties
        public Transform PlayerTransform
        {
            get { return playerTransform; }
        }

        public Transform CameraPositionTransform
        {
            get { return cameraPositionTransform; }
        }

        public Collider PlayerPhysicsCollider
        {
            get { return playerPhysicsCollider; }
        }
        #endregion

        #region PlayerJump
        public bool IsJumping { get; set; }
        public Action JumpStarted;
        #endregion

        #region PlayerThrowGrenade
        public Action<IGrenade> OnThrowGrenade;
        #endregion

        #region PlayerInteract
        public Action<WeaponData> OnChangeWeapon;
        #endregion

        #region PlayerShoot + Aim
        public bool IsAiming { get; set; }
        #endregion

        #region Mono
        private void Awake()
        {
            abilities = GetComponentsInChildren<PlayerAbilityBase>();
            foreach (var ability in abilities)
            {
                ability.Init(this, playerVisual);
                ability.enabled = true;
            }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate()
        {
            playerVisual.SetAnimatorParameter(currentSpeedParameter, GetDistanceSquared(velocityX: playerRigidbody.velocity.x, velocityZ: playerRigidbody.velocity.z));
        }
        #endregion

        #region RigidbodyMethods 
        public Vector3 GetVelocity()
        {
            return playerRigidbody.velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            playerRigidbody.velocity = velocity;
        }

        public void SetImpulse(Vector3 impulse)
        {
            SetVelocity(Vector3.zero);
            playerRigidbody.AddForce(impulse, ForceMode.Impulse);
        }
        #endregion

        #region PublicMethods
        public float GetDistanceSquared(float velocityX = 0f, float velocityY = 0f, float velocityZ = 0f)
        {
            return Math.Abs((velocityX * velocityX) + (velocityY + velocityY) + (velocityZ * velocityZ));
        }
        #endregion

        #region PrivateMethods
        private void DisableInput()
        {
            foreach (var ability in abilities)
            {
                ability.OnInputDisabled();
            }
        }

        private void EnableInput()
        {
            foreach (var ability in abilities)
            {
                ability.OnInputEnabled();
            }
        }

        #endregion
        #region Interface IDamageble
        public void TakeDamage(DamageContainer damage)
        {
            InternalTakeDamage(damage);
            if (healthModule.IsDead) { Debug.Log("PlayerIsDead"); }
        }
        #endregion
        #region HealthModule
        private void InternalTakeDamage(DamageContainer damage)
        {
            healthModule.TakeDamage(damage);
        }
        #endregion
    }
}