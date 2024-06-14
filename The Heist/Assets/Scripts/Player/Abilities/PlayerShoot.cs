using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : PlayerAbilityBase, IShooter
{
    private const string aimStringParameter = "Aim";

    #region Variables
    [SerializeField]
    private Transform socketShoot;

    private WeaponData currentWeaponData;
    private int currentAmmo;

    private float reloadTimer=0.0f;
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

            if (reloadTimer <= 0f)
            {
                currentAmmo = currentWeaponData.MaxAmmo;
                canShoot = true;
            }
        }

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
            Vector3 direction = initialPosition + (playerController.CameraPositionTransform.forward * currentWeaponData.Range);

            Shoot(initialPosition, direction, currentWeaponData.TypeOfShoot);
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

    private void ComputeShootRange(Vector3 direction)
    {
        Vector3 contactPoint = Vector3.zero;

        // Questi due vettori andranno sottratti per trovare ufficialmente la direction del bullet.
        if (Physics.Linecast(socketShoot.position, direction, out RaycastHit hit))
        {
            contactPoint = hit.point;
            Debug.Log("Colpito!" + hit.collider.gameObject.name);

            Debug.DrawLine(socketShoot.position, contactPoint, Color.red, 0.1f);
            //Debug.DrawLine(initialPosition, contactPoint, Color.blue, 30f);

            IDamageble damageble = hit.collider.gameObject.GetComponent<IDamageble>();
            if (damageble == null) return;
            damageble.TakeDamage(currentWeaponData.DamageContainer);
        }
        else
        {
            contactPoint = playerController.CameraPositionTransform.forward * currentWeaponData.Range;
            Debug.Log("Non Colpito!");

            Debug.DrawLine(socketShoot.position, direction, Color.red, 0.1f);
            //Debug.DrawLine(initialPosition, playerController.CameraPositionTransform.position + contactPoint, Color.blue, 30f);
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
        if (currentAmmo >= currentWeaponData.MaxAmmo || reloadTimer > 0f) return;
        canShoot = false;
        reloadTimer = currentWeaponData.ReloadTime;
    }

    public void Shoot(Vector3 initialPosition, Vector3 direction, ShootType currentShootType)
    {
        currentAmmo--;
        canShoot = false;
        fireTime = currentWeaponData.RateOfFire;

        switch (currentShootType)
        {
            case ShootType.Single:
                if (hasShot) return;
                ComputeShootRange(direction);
            break;
            case ShootType.Multiple:
                hasMultiShot = true;
                ComputeShootRange(direction);
            break;
            case ShootType.Shotgun:
                // To do
            break;
            default: return;
        }

    }
    #endregion
}