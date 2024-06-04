using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private WeaponData weaponData;

    private int currentAmmo;

   /* [SerializeField]
    private float randomRange; */

    private bool canShoot;
    private float reloadTimer = 0f;
    private float fireTime = 0f;

   // private Vector3[] directions = new Vector3[10];


    public void Start()
    {
        currentAmmo = weaponData.MaxAmmo;
        canShoot = true;
        // Initialize directions array
       /* for (int i = 0; i < directions.Length; i++)
        {
            // Generate random directions within a specific range
            float randomX = UnityEngine.Random.Range(-randomRange, randomRange);
            float randomY = UnityEngine.Random.Range(-randomRange, randomRange);
            float randomZ = UnityEngine.Random.Range(-randomRange, randomRange);
            directions[i] = new Vector3(randomX, randomY, randomZ);
        } */
    }

    public void Reload()
    {
        if (currentAmmo >= weaponData.MaxAmmo || reloadTimer > 0f)
        {
            return;
        }

        canShoot = false;
        reloadTimer = weaponData.ReloadTime;
    }

    public void Shoot(Vector3 initialPosition, Vector3 direction)
    {
        if (canShoot && currentAmmo > 0)
        {
            currentAmmo--;
            fireTime = weaponData.RateOfFire;
            canShoot = false;

            RaycastHit hit;
            // int randomDirectionIndex = UnityEngine.Random.Range(0, directions.Length); 
            //  Vector3 randomDirection = directions[randomDirectionIndex];
            Vector3 finalDirection = direction;// + randomDirection;

            if (Physics.Raycast(initialPosition, finalDirection, out hit, weaponData.Range))
            {
                if (hit.collider.gameObject.GetComponent<IDamageble>() == null && hit.collider.gameObject == gameObject)
                {
                    Vector3 endPosition = initialPosition + finalDirection * weaponData.Range;
                    Debug.DrawLine(initialPosition, endPosition, Color.red, 0.2f);
                    return;
                }
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Hit Player!!!");
                    Debug.DrawLine(initialPosition, hit.point, Color.green, 0.2f);
                }
            }
            else
            {
                Vector3 endPosition = initialPosition + finalDirection * weaponData.Range;
                Debug.DrawLine(initialPosition, endPosition, Color.red, 0.2f);
            }

        }
        else Reload();
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

            if (fireTime <= 0f)
            {
                canShoot = true;
            }
        }
    }
}
