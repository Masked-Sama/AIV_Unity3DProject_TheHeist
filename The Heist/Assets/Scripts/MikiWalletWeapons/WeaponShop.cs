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

    private PlayerInputActions inputActions;
    private bool isInteracting;

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
            if (weapon != null)
            {
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
         //playerCurrency = GetComponent<PlayerCurrency>();
         PlayerCurrency soldi = playerCurrency.GetComponent<PlayerCurrency>();

        if (soldi != null)
        {
            Debug.Log("Player current money: " + soldi.money);
            Debug.Log("Weapon cost: " + weapon.cost);

            if (soldi.SpendMoney(weapon.cost))
            {
                weapon.EquipWeapon();
            }
            else
            {
                Debug.Log("Insufficient funds.");
            }
        }
        else
        {
            Debug.Log("PlayerCurrency component not found on player.");
        }
    }


    private void EquipWeapon(Weapon weapon)
    {
        // Disattiva tutte le altre armi (se presenti)
        foreach (Transform child in transform)
        {
            Weapon childWeapon = child.GetComponent<Weapon>();
            if (childWeapon != null)
            {
                childWeapon.gameObject.SetActive(false);
            }
        }

        // Attiva l'arma acquistata
        weapon.gameObject.SetActive(true);

        // Logica di equipaggiamento aggiuntiva
        Debug.Log($"Equipped {weapon.weaponName}");
    }
}
