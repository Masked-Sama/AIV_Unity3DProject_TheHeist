using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Text infoText;
    public PlayerCurrency playerCurrency;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("Main Camera not found! Please ensure your player camera is tagged as MainCamera.");
        }
        infoText.text = "";
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Genera un Ray dal centro della telecamera (che corrisponde alla posizione del crosshair)
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
            Weapon weapon = hit.transform.GetComponent<Weapon>();
            if (weapon != null)
            {
                infoText.text = weapon.weaponName + " - Costo: " + weapon.cost;
                Debug.Log("Puntamento su: " + weapon.weaponName);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Tentativo di acquisto: " + weapon.weaponName);
                    weapon.BuyWeapon(playerCurrency);
                }
            }
            else
            {
                infoText.text = "";
                Debug.Log("Raycast hit non-weapon object: " + hit.transform.name);
            }
        }
        else
        {
            infoText.text = "";
            Debug.Log("Raycast did not hit any object");
        }
    }
}
