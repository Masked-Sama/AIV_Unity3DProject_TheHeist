using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : PlayerAbilityBase, IShooter
{
    private WeaponData currentWeaponData;
    private int currentAmmo;

    private float reloadTimer;
    private float fireTime;

    private bool canShoot = true;

    #region Mono
    private void OnEnable()
    {
        InputManager.Player.Shoot.performed += OnShootPerformed;
        playerController.OnChangeWeapon += ChangeWeapon;
    }

    private void OnDisable()
    {
        InputManager.Player.Shoot.performed -= OnShootPerformed;
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
            }
        }
    }
    #endregion

    #region PrivateMethods
    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (CanShoot())
        {
            Vector3 initialPosition = playerController.CameraPositionTransform.position;
            Vector3 direction = initialPosition + (playerController.CameraPositionTransform.forward * currentWeaponData.Range);

            Shoot(initialPosition, direction);
        }
        else if(fireTime <= 0f)
        {
            Reload();
        }
    }

    private bool CanShoot()
    {
        return !isPrevented && currentAmmo > 0 && canShoot;
    }

    private void ChangeWeapon(WeaponData newWeapon)
    {
        if (currentWeaponData == newWeapon) return;
        currentWeaponData = newWeapon;
        currentAmmo = newWeapon.MaxAmmo;
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

    public void Shoot(Vector3 initialPosition, Vector3 direction)
    {
        currentAmmo--;
        canShoot = false;
        fireTime = currentWeaponData.RateOfFire;

        Vector3 contactPoint = Vector3.zero; 

        // Questi due vettori andranno sottratti per trovare ufficialmente la direction del bullet.
        if (Physics.Linecast(playerController.PlayerTransform.position, direction, out RaycastHit hit))
        {
            contactPoint = hit.point;
            Debug.Log("Colpito!" + hit.collider.gameObject.name);

            Debug.DrawLine(playerController.PlayerTransform.position, contactPoint, Color.red, 30f);
            Debug.DrawLine(initialPosition, contactPoint, Color.blue, 30f);
        }
        else
        {
            contactPoint = playerController.CameraPositionTransform.forward * currentWeaponData.Range;
            Debug.Log("Non Colpito!");

            Debug.DrawLine(playerController.PlayerTransform.position, direction, Color.red, 30f);
            Debug.DrawLine(initialPosition, playerController.CameraPositionTransform.position + contactPoint, Color.blue, 30f);
        }

    }
    #endregion
}
