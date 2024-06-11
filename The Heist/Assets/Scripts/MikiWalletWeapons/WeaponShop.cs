using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShop : MonoBehaviour
{
    public Transform playerCamera;
    public float interactionDistance = 3f;
    public LayerMask weaponLayer;
    public GameObject infoText;
    [SerializeField]
    public PlayerCurrency playerCurrency;
    public Transform weaponHoldPoint; // Punto di attacco per l'arma sul giocatore

    private PlayerInputActions inputActions;
    private bool isInteracting;
    private Weapon currentWeapon;
    private Weapon equippedWeapon; // L'arma attualmente equipaggiata
    private Weapon lastWeaponSold;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += OnInteractPerformed;
        inputActions.Player.Interact.canceled += OnInteractCanceled;

    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Player.Interact.canceled -= OnInteractCanceled;
        inputActions.Disable();

    }



    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        isInteracting = true;
        Debug.Log("Interact performed");
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        isInteracting = false;
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, interactionDistance, weaponLayer))
        {
            Weapon weapon = hit.collider.GetComponent<Weapon>();
            if (weapon != null && weapon != equippedWeapon) // Impedisce di acquistare l'arma equipaggiata
            {
                currentWeapon = weapon;
                infoText.SetActive(true);
                infoText.GetComponent<UnityEngine.UI.Text>().text = $"{weapon.weaponName} - Cost: {weapon.cost}";

                if (isInteracting)
                {
                    PurchaseWeapon(weapon);
                    isInteracting = false; // Reset interaction flag to prevent multiple purchases
                }
            }
        }
        else
        {
            infoText.SetActive(false);
        }
    }

    private void PurchaseWeapon(Weapon weapon)
    {
        PlayerCurrency soldi = playerCurrency.GetComponent<PlayerCurrency>();

        if (soldi != null)
        {
            Debug.Log("Player current money: " + soldi.money);
            Debug.Log("Weapon cost: " + weapon.cost);

            if (soldi.SpendMoney(weapon.cost))
            {
                // Riattiva l'arma precedente nel negozio se esiste
                if (equippedWeapon != null)
                {
                    equippedWeapon.gameObject.SetActive(true);


                }

                EquipWeapon(weapon);
                // Disattiva l'arma acquistata
                weapon.gameObject.SetActive(false);
                if (lastWeaponSold != null)
                {
                    lastWeaponSold.gameObject.SetActive(true);
                }
                lastWeaponSold = weapon;
            }



        }
    }

    private void EquipWeapon(Weapon weapon)
    {
        // Disattiva tutte le armi precedenti sul giocatore
        foreach (Transform child in weaponHoldPoint)
        {
            Destroy(child.gameObject);
        }

        // Instanzia la nuova arma e attaccala al giocatore
        GameObject newWeaponInstance = Instantiate(weapon.gameObject, weaponHoldPoint);
        newWeaponInstance.transform.localPosition = Vector3.zero;
        newWeaponInstance.transform.localRotation = Quaternion.identity;

        // Imposta la nuova arma come l'arma equipaggiata
        equippedWeapon = newWeaponInstance.GetComponent<Weapon>();

        // Logica di equipaggiamento aggiuntiva
        Debug.Log($"Equipped {weapon.weaponName}");


        currentWeapon = weapon;
    }
}


