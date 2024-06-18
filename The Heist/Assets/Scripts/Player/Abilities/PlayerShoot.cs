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
        [SerializeField]
        private GameObject textReloading;

        private WeaponData currentWeaponData;
        private int currentWeaponIndex;

        private int currentAmmoInMagazine;

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
            if (reloadTimer > 0f)   //se il tempo di ricarica non è ancora finito, decremento e ritorno
            {
                reloadTimer -= Time.fixedDeltaTime;
                textReloading.GetComponent<UnityEngine.UI.Text>().text = "Reloading " + ((short)reloadTimer);
                return;
            }
            if (fireTime > 0f)      //se il tempo tra uno sparo e l'altro non è ancora finito, decremento e ritorno
            {
                fireTime -= Time.fixedDeltaTime;
                return;
            }
            if (!canShoot)
            {
                canShoot = true;    //altrimenti posso sparare
                textReloading.SetActive(false);
            }

            if (!hasMultiShot) return;
            InternalOnShootPerformed(); //se sto sparando con un arma multishot, continuo a sparare
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
            //Posso sparare quando sto mirando && ho almeno un proiettile && il cooldown tra un colpo e l'altro è 0
            if (CanShoot())
            {
                Vector3 initialPosition = playerController.CameraPositionTransform.position;
                Vector3 finalPosition = initialPosition + (playerController.CameraPositionTransform.forward * currentWeaponData.Range);

                Shoot(initialPosition, finalPosition, currentWeaponData.TypeOfShoot);
            }
            else if (fireTime <= 0f)    //Se invece non posso sparare  il cooldown è 0, allora Faccio il reload
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
                && currentAmmoInMagazine>0
                && canShoot
                && isAiming;
        }

        private void ChangeWeapon(WeaponData newWeapon)
        {
            if (currentWeaponData == newWeapon) return;
            currentWeaponData = newWeapon;
            currentWeaponIndex = playerController.Inventory.FindWeaponSlot(currentWeaponData);
            ReloadCurrentAmmo();
        }

        private void ReloadCurrentAmmo()
        {
            currentAmmoInMagazine = playerController.Inventory.InventorySlots[currentWeaponIndex].Amount >= currentWeaponData.MaxAmmoForMagazine
                        ? currentWeaponData.MaxAmmoForMagazine : playerController.Inventory.InventorySlots[currentWeaponIndex].Amount;
        }

        private void ComputeShootRange(Vector3 initialPosition, Vector3 finalPosition)
        {
            Vector3 contactPoint = Vector3.zero;

            // Questi due vettori andranno sottratti per trovare ufficialmente la direction del bullet.
            if (Physics.Linecast(initialPosition, finalPosition, out RaycastHit hit))
            {
                contactPoint = hit.point;
                Debug.DrawLine(socketShoot.position, contactPoint, Color.red, .1f); // SARA' QUESTA LA DIRECTION DEL BULLET!

                IDamageble damageble = hit.collider.gameObject.GetComponent<IDamageble>();
                if (damageble == null) return;
                damageble.TakeDamage(currentWeaponData.DamageContainer);
            }
            else
            {
                contactPoint = finalPosition;
                Debug.DrawLine(socketShoot.position, finalPosition, Color.red, .1f);
            }

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
            //if (playerController.Inventory.InventorySlots[currentWeaponIndex].Amount >= currentWeaponData.MaxAmmoForMagazine || reloadTimer > 0f) return;
            if (currentAmmoInMagazine > 0 || reloadTimer > 0f) return;
            canShoot = false;
            ReloadCurrentAmmo();
            if(currentAmmoInMagazine > 0)
            textReloading.SetActive(true);
            reloadTimer = currentWeaponData.ReloadTime;
        }

        public bool Shoot(Vector3 initialPosition, Vector3 finalPosition, ShootType currentShootType)
        {
            canShoot = false;
            fireTime = currentWeaponData.RateOfFire;
            currentAmmoInMagazine--;
            
            //To change when shotgun logic is implemented
            GlobalEventManager.CastEvent(GlobalEventIndex.Shoot, GlobalEventArgsFactory.ShootFactory(currentWeaponData.Prefab, 1));
            playerController.Inventory.InventorySlots[currentWeaponIndex].AddAmount(-1);

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
