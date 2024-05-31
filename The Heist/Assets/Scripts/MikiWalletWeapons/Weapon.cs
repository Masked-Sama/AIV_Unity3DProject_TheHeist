using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int cost;
    public GameObject weaponPrefab;

    private Transform weaponSlot;

    public void Start()
    {
        weaponSlot = GameObject.FindWithTag("WeaponSlot").transform;
    }

    public void BuyWeapon(PlayerCurrency playerCurrency)
    {
        if (playerCurrency.CanAfford(cost))
        {
            playerCurrency.CanAfford(cost);
            EquipWeapon();
        }
        else
        {
            Debug.Log("Non hai i cash");
        }
    }

    public void EquipWeapon()
    {
        Debug.Log(weaponName + " acquistata e equippagiata!");

        foreach (Transform child in weaponSlot)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject equippedWeapon = Instantiate(weaponPrefab, weaponSlot);
        equippedWeapon.transform.localPosition = Vector3.zero;
        equippedWeapon.transform.localRotation = Quaternion.identity;
    }
}
