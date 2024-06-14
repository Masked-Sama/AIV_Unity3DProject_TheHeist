using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    
    private GameObject lastFirstWeaponSold;
    private GameObject lastSecondWeaponSold;
    
    private void OnEnable()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.BuyItem, BuyItem);
    }

    private void BuyItem(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.BuyItemParser(message, out GameObject itemSold);
        WeaponData item = (WeaponData)itemSold.GetComponent<Item>().ItemData;
        if (item == null) return;

        if (item.GetType() == typeof(FirstWeaponData))
        {
            itemSold.SetActive(false);
            if (lastFirstWeaponSold != null)
            {
                lastFirstWeaponSold.SetActive(true);
            }
            lastFirstWeaponSold = itemSold;
        }else if (item.GetType() == typeof(SecondWeaponData))
        {
            itemSold.SetActive(false);
            if (lastSecondWeaponSold != null)
            {
                lastSecondWeaponSold.SetActive(true);
            }
            lastSecondWeaponSold = itemSold;
        }
    

    }
}


