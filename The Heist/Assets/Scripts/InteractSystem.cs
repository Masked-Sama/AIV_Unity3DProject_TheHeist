using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    [SerializeField]
    private Transform point1Transform;
    [SerializeField]
    private Transform point2Transform;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float distance;
    [SerializeField]
    private GameObject textUI;
    [SerializeField]
    private InventoryObject inventory;

    private InputAction interact;
    private SimpleMovementComponent movementComponent;
    private bool canPickUp = false;
    private GameObject lastObjCollided;


    private void Awake()
    {
        movementComponent = GetComponent<SimpleMovementComponent>();
    }

    private void OnEnable()
    {
        InputManager.Player.Interact.performed += OnInteract;
    }
    private void OnDisable()
    {
        InputManager.Player.Interact.performed -= OnInteract;
    }

    private void Update()
    {
        RaycastHit hit;
        
        Vector3 p1 = point1Transform.position;
        Vector3 p2 = point2Transform.position;

        textUI.SetActive(false);        
        if (Physics.CapsuleCast(p1, p2, radius, transform.forward, out hit, distance))
        {
            lastObjCollided = hit.collider.gameObject;
            textUI.SetActive(true);
            canPickUp = true;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {

        if (!canPickUp) return;
        Debug.Log("lastObjCollided: " + lastObjCollided.name);
        
        GlobalEventManager.CastEvent(GlobalEventIndex.AddItemToInventory, GlobalEventArgsFactory.AddItemToInventoryFactory(lastObjCollided));
        //var item = lastObjCollided.GetComponent<Item>();
        //if (item != null)
        //{
        //    inventory.AddItem(item.ItemObject, 1);
        //}
    }

    private void OnDrawGizmos()
    {
        if (point1Transform == null || point2Transform == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(point1Transform.position, radius);
        Gizmos.DrawWireSphere(point2Transform.position, radius);
        Gizmos.DrawLine(point1Transform.position, point2Transform.position);
        Gizmos.DrawRay(point1Transform.position, transform.forward * distance);
    }
}
