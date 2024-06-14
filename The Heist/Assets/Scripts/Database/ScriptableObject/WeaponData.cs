using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootType
{
    Single,
    Multiple,
    Shotgun
}


public class WeaponData : ItemData
{
    [SerializeField] private int maxAmmo; 
    [SerializeField] private float rateOfFire;
    [SerializeField] private DamageContainer damageContainer; 
    [SerializeField] private float randomDirection; 
    [SerializeField] private ShootType typeOfShoot; 
    [SerializeField] private float reloadTime; 
    [SerializeField] private float range;
    [SerializeField] private Texture2D texture;
    public int MaxAmmo
    {
        get { return maxAmmo; }
    }
    public float RateOfFire
    {
        get { return rateOfFire; }
    }
    public DamageContainer DamageContainer
    {
        get { return damageContainer; }
    }
    public float RandomDirection
    {
        get { return randomDirection; }
    }
    public ShootType TypeOfShoot
    {
        get { return typeOfShoot; }
    }
    public float ReloadTime
    {
        get { return reloadTime; }
    }
    public float Range 
    { 
        get { return range; }
    }
    public Texture2D Texture 
    {
        get { return texture; }
    }

    
}



