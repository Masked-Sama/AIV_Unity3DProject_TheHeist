using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int cost;
    public GameObject weaponPrefab;

    private Transform weaponSlot;
    [SerializeField]
    private WeaponData weaponData;

    //public void Start()
    //{
    //    weaponSlot = GameObject.FindWithTag("WeaponSlot").transform;
    //}

    

    public void EquipWeapon()
    {
        Debug.Log(weaponName + " acquistata e equippagiata!");

        foreach (Transform child in weaponSlot)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject equippedweapon = Instantiate(weaponPrefab, weaponSlot);
        equippedweapon.transform.localPosition = Vector3.zero;
        equippedweapon.transform.localRotation = Quaternion.identity;
    }
}
