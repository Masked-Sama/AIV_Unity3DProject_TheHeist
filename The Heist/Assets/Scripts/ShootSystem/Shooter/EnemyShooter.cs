using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;


public class EnemyShooter : MonoBehaviour, IShooter
{
    [SerializeField]
    private WeaponData weaponData;

    public WeaponData WeaponData
    {
        get { return weaponData; }
    }

    private int currentAmmo;

    [SerializeField]
    private float randomRange;

    private bool canShoot;
    private float reloadTimer = 0f;
    private float fireTime = 0f;

    private Vector3[] directions = new Vector3[10];

    private Animator animator;

    public void Start()
    {
        currentAmmo = weaponData.MaxAmmo;
        canShoot = true;
        // Initialize directions array
        for (int i = 0; i < directions.Length; i++)
        {
            // Generate random directions within a specific range
            float randomX = UnityEngine.Random.Range(-randomRange, randomRange);
            float randomY = UnityEngine.Random.Range(-randomRange, randomRange);
            float randomZ = UnityEngine.Random.Range(-randomRange, randomRange);
            directions[i] = new Vector3(randomX, randomY, randomZ);
        }
        animator = GetComponent<Animator>();
    }

    public void Reload()
    {
        if (currentAmmo >= weaponData.MaxAmmo || reloadTimer > 0f)
        {
            return; 
        }
        animator.SetTrigger("Reload");
        canShoot = false;
        reloadTimer = weaponData.ReloadTime;
    }

    public void Shoot(Vector3 initialPosition, Vector3 direction, ShootType WeaponData)
    {
        if (canShoot && currentAmmo > 0)
        {
            currentAmmo--;
            fireTime = weaponData.RateOfFire;
            canShoot = false;

            RaycastHit hit;
            int randomDirectionIndex = UnityEngine.Random.Range(0, directions.Length);
            Vector3 randomDirection = directions[randomDirectionIndex];
            Vector3 finalDirection = direction + randomDirection;

            if (Physics.Raycast(initialPosition, finalDirection, out hit, weaponData.Range))
            {
                if (hit.collider.gameObject.GetComponent<IDamageble>() == null && hit.collider.gameObject == gameObject)
                {
                    Vector3 endPosition = initialPosition + finalDirection * weaponData.Range;
                    Debug.DrawLine(initialPosition, endPosition, Color.red, 0.1f);
                    return;
                }
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Hit Player!!!");
                    Debug.DrawLine(initialPosition, hit.point, Color.green, 0.1f);
                }
            }
            else
            {
                Vector3 endPosition = initialPosition + finalDirection * weaponData.Range;
                Debug.DrawLine(initialPosition, endPosition, Color.red, 0.1f);
            }

        }
        else if (fireTime <= 0f) Reload();
    }


    public void Update()
    {
        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;

            if (reloadTimer <= 0f)
            {
                currentAmmo = weaponData.MaxAmmo;
                canShoot = true;
            }
        }

        if (fireTime > 0f)
        {
            fireTime -= Time.deltaTime;

            if ( fireTime <= 0f)
            {
                canShoot = true;
            }
        }
    }

}
