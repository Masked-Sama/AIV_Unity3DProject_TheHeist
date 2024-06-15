using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.YamlDotNet.Core;
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

    private Transform boneWeapon;

    private bool isReloading = false;


    #region Mono
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

        Transform[] allBones = gameObject.GetComponentsInChildren<Transform>();
        boneWeapon = FindBone(allBones, "WeaponHand_R");

        GameObject instance = Instantiate(WeaponData.Prefab, boneWeapon.position, boneWeapon.rotation);

        instance.transform.SetParent(boneWeapon);
    }

    public void FixedUpdate()
    {
        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;

            if (reloadTimer <= 0f)
            {
                currentAmmo = weaponData.MaxAmmo;
                canShoot = true;
                isReloading = false;
            }
        }

        if (fireTime > 0f)
        {
            fireTime -= Time.deltaTime;

            if (fireTime <= 0f)
            {
                canShoot = true;
            }
        }
    }
    #endregion

    private Transform FindBone(Transform[] allBones, string boneName)
    {
        foreach (Transform bone in allBones)
        {
            if (bone.name == boneName)
            {
                return bone;
            }
        }
        return null; // Bone not found
    }

    #region IShooter
    public void Reload()
    {
        if (currentAmmo >= weaponData.MaxAmmo || reloadTimer > 0f)
        {
            return;
        }
        animator.SetTrigger("Reload");
        canShoot = false;
        isReloading = true;
        reloadTimer = weaponData.ReloadTime;
    }

    public bool Shoot(Vector3 initialPosition, Vector3 direction, ShootType WeaponData)
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

                }
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    //Debug.Log("Hit Player!!!");
                    IDamageble playerDamager = hit.collider.gameObject.GetComponent<IDamageble>();
                    playerDamager.TakeDamage(weaponData.DamageContainer);
                    Debug.DrawLine(initialPosition, hit.point, Color.green, 0.1f);
                }
            }
            else
            {
                Vector3 endPosition = initialPosition + finalDirection * weaponData.Range;
                Debug.DrawLine(initialPosition, endPosition, Color.red, 0.1f);
            }

            

        }
        else if (fireTime <= 0f)
        {
            Reload();
        }

        return isReloading;

    }

    

    #endregion


}
