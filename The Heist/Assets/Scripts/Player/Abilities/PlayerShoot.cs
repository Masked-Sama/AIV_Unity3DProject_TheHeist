using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerShoot : PlayerAbilityBase, IShooter
    {
        #region Variables
        [SerializeField]
        private Transform socketShoot;

        private WeaponData currentWeaponData;
        private int currentWeaponIndex;

        private int currentAmmoInMagazine;

        private float reloadTimer = 0.0f;
        private float fireTime;

        private bool canShoot = true;

        private bool hasShot = false;
        private bool hasMultiShot = false;
        private GameObject weaponVisual;
        #endregion

        #region Mono
        private void OnEnable()
        {
            InputManager.Player.Shoot.performed += OnShootPerformed;
            InputManager.Player.Shoot.canceled += OnShootCanceled;
            playerController.OnChangeWeapon += ChangeWeapon;
            playerController.OnPickUpItem += PickUpWeapon;
        }

        private void OnDisable()
        {
            InputManager.Player.Shoot.performed -= OnShootPerformed;
            InputManager.Player.Shoot.canceled -= OnShootCanceled;
            playerController.OnChangeWeapon -= ChangeWeapon;
            playerController.OnPickUpItem -= PickUpWeapon;
        }

        private void FixedUpdate()
        {
            if (reloadTimer > 0f)   //se il tempo di ricarica non è ancora finito, decremento e ritorno
            {
                reloadTimer -= Time.fixedDeltaTime;
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

        private void OnShootCanceled(InputAction.CallbackContext context)
        {
            hasShot = false;
            hasMultiShot = false;
        }

        private bool CanShoot()
        {
            return !isPrevented
                && currentAmmoInMagazine > 0
                && canShoot
                && playerController.IsAiming;
        }

        private void ChangeWeapon(WeaponData newWeapon)
        {
            if (currentWeaponData == newWeapon) return;
            currentWeaponData = newWeapon;
            currentWeaponIndex = playerController.Inventory.FindWeaponSlot(currentWeaponData);
            ReloadCurrentAmmo();
            VisualChangeWeapon(newWeapon);
        }
        private void VisualChangeWeapon(WeaponData data)
        {
            Destroy(weaponVisual);
            weaponVisual = Instantiate(data.Prefab, playerController.BoneWeapon.position, playerController.BoneWeapon.rotation);
            weaponVisual.transform.SetParent(playerController.BoneWeapon);
        }

        private void PickUpWeapon(ItemData item)
        {
            if (!(item is WeaponData) || currentWeaponData == null) return;      //Se non sto raccogliendo un arma (o se non ho niente in mano), ritorno
            WeaponData newWeapon = item as WeaponData;
            if (newWeapon.ItemType != currentWeaponData.ItemType) return;
            //continua solo se l'attuale arma è dello stesso tipo di quella raccolta ma non è la stessa arma
            ChangeWeapon(newWeapon);
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

        public override void StopAbility() { }
        #endregion

        #region IShooter
        public void Reload()
        {
            if (currentAmmoInMagazine > 0 || reloadTimer > 0f) return;
            canShoot = false;
            ReloadCurrentAmmo();
            if (currentAmmoInMagazine > 0)
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
