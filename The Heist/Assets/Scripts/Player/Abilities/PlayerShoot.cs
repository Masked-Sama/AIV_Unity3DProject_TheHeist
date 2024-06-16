using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerShoot : PlayerAbilityBase, IShooter
    {
        private const string aimStringParameter = "Aim";

        #region Variables
        [SerializeField]
        private Transform socketShoot;

        private WeaponData currentWeaponData;
        private int currentAmmo;

        private float reloadTimer = 0.0f;
        private float fireTime;

        private bool canShoot = true;
        private bool isAiming = false;
        private bool hasShot = false;

        private bool hasMultiShot = false;
        #endregion

        #region Mono
        private void OnEnable()
        {
            InputManager.Player.Shoot.performed += OnShootPerformed;
            InputManager.Player.Shoot.canceled += OnShootCanceled;
            InputManager.Player.Aim.performed += OnAimPerformed;
            InputManager.Player.Aim.canceled += OnAimCanceled;
            playerController.OnChangeWeapon += ChangeWeapon;
        }

        private void OnDisable()
        {
            InputManager.Player.Shoot.performed -= OnShootPerformed;
            InputManager.Player.Shoot.canceled -= OnShootCanceled;
            InputManager.Player.Aim.performed -= OnAimPerformed;
            InputManager.Player.Aim.canceled -= OnAimCanceled;
            playerController.OnChangeWeapon -= ChangeWeapon;
        }

        private void FixedUpdate()
        {
            if (reloadTimer > 0f)
            {
                reloadTimer -= Time.fixedDeltaTime;

                if (reloadTimer <= 0f) // && playerController.Inventory.InventorySlots[playerController.Inventory.FindWeaponSlot(currentWeaponData)].Amount>0)
                {
                    // if(playerController.Inventory.InventorySlots[playerController.Inventory.FindWeaponSlot(currentWeaponData)].Amount>= currentWeaponData.MaxAmmo)
                    //     currentAmmo = currentWeaponData.MaxAmmo;
                    canShoot = true;
                }
            }
            currentAmmo = playerController.Inventory.InventorySlots[playerController.Inventory.FindWeaponSlot(currentWeaponData)].Amount;

            if (fireTime > 0f)
            {
                fireTime -= Time.fixedDeltaTime;

                if (fireTime <= 0f)
                {
                    canShoot = true;
                    if (!hasMultiShot) return;
                    InternalOnShootPerformed();
                }
            }
        }
        #endregion

        #region PrivateMethods
        private void OnShootPerformed(InputAction.CallbackContext context)
        {
            InternalOnShootPerformed();
        }

        private void InternalOnShootPerformed()
        {
            if (currentWeaponData == null) return;
            if (CanShoot())
            {
                Vector3 initialPosition = playerController.CameraPositionTransform.position;
                Vector3 finalPosition = initialPosition + (playerController.CameraPositionTransform.forward * currentWeaponData.Range);

                Shoot(initialPosition, finalPosition, currentWeaponData.TypeOfShoot);
            }
            else if (fireTime <= 0f)
            {
                Reload();
            }
        }

        private void OnAimPerformed(InputAction.CallbackContext context)
        {
            isAiming = true;
            playerVisual.SetAnimatorParameter(aimStringParameter, isAiming);
        }

        private void OnAimCanceled(InputAction.CallbackContext context)
        {
            isAiming = false;
            playerVisual.SetAnimatorParameter(aimStringParameter, isAiming);
        }

        private void OnShootCanceled(InputAction.CallbackContext context)
        {
            hasShot = false;
            hasMultiShot = false;
        }

        private bool CanShoot()
        {
            return !isPrevented
                && currentAmmo > 0
                && canShoot
                && isAiming;
        }

        private void ChangeWeapon(WeaponData newWeapon)
        {
            if (currentWeaponData == newWeapon) return;
            currentWeaponData = newWeapon;
            currentAmmo = newWeapon.MaxAmmo;
        }

        private void ComputeShootRange(Vector3 initialPosition, Vector3 finalPosition)
        {
            Vector3 contactPoint = Vector3.zero;

            // Questi due vettori andranno sottratti per trovare ufficialmente la direction del bullet.

            if (Physics.Linecast(initialPosition, finalPosition, out RaycastHit hit))
            {
                contactPoint = hit.point;
                //Debug.Log("Colpito!" + hit.collider.gameObject.name);

                Debug.DrawLine(socketShoot.position, contactPoint, Color.red, .1f); // SARA' QUESTA LA DIRECTION DEL BULLET!
                //Debug.DrawLine(initialPosition, contactPoint, Color.blue, 30f);

                IDamageble damageble = hit.collider.gameObject.GetComponent<IDamageble>();
                if (damageble == null) return;
                damageble.TakeDamage(currentWeaponData.DamageContainer);
            }
            else
            {
                contactPoint = finalPosition;
                //Debug.Log("Non Colpito!");

                Debug.DrawLine(socketShoot.position, finalPosition, Color.red, .1f);
                //Debug.DrawLine(initialPosition, finalPosition, Color.blue, 30f);
            }

            // Poi da qui usare il linecast finale per dare la direction al bullet.
        }
        #endregion

        #region ShootTypes
        private void ShotgunShot()
        {

        }
        #endregion

        #region InputMethods
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
        #endregion

        #region IShooter
        public void Reload()
        {
            if (currentAmmo >= currentWeaponData.MaxAmmo || reloadTimer > 0f) return;
            canShoot = false;
            reloadTimer = currentWeaponData.ReloadTime;
        }

        public bool Shoot(Vector3 initialPosition, Vector3 finalPosition, ShootType currentShootType)
        {
            // currentAmmo--;
            canShoot = false;
            fireTime = currentWeaponData.RateOfFire;
            
            Debug.Log(currentAmmo);
            
            //To change when shotgun logic is implemented
            GlobalEventManager.CastEvent(GlobalEventIndex.Shoot, GlobalEventArgsFactory.ShootFactory(currentWeaponData.Prefab, 1));

            //Debug.Log(currentWeaponData.name);
            playerController.Inventory.InventorySlots[playerController.Inventory.FindWeaponSlot(currentWeaponData)].AddAmount(-1);

            //Debug.DrawLine(socketShoot.position, finalPosition, Color.magenta, 30);

            switch (currentShootType)
            {
                case ShootType.Single:
                    if (hasShot) return true;
                    ComputeShootRange(initialPosition, finalPosition);
                    break;
                case ShootType.Multiple:
                    hasMultiShot = true;
                    ComputeShootRange(initialPosition, finalPosition);
                    break;
                case ShootType.Shotgun:
                    // To do
                    break;
                default: return true;
            }
            return true;
        }
        #endregion
    }
}
