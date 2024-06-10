using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Player
{
    public class PlayerInteract : PlayerAbilityBase
    {
        [SerializeField]
        private float radius;
        [SerializeField]
        private float distance;
        [SerializeField]
        private Transform cameraTransform;

        [SerializeField]
        private Transform origin;

        [SerializeField]
        private GameObject textUI;
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private LayerMask wallMask;
        [SerializeField]
        private InventoryObject playerInventory;

        private GameObject itemDetected;
        private Action onItemDetected;
        private Action onItemUndetected;
        private RaycastHit hit;
        private bool canInteract = false;

        #region Override
        public override void OnInputDisabled()
        {
            isPrevented = true;
        }
        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility()
        {
        
        }
        #endregion

        #region Mono
        private void OnEnable()
        {
            InputManager.Player.Interact.performed += OnInteractPerform;
            onItemUndetected += ItemUndetected;
            onItemDetected += ItemDetected;
        }
        private void OnDisable()
        {
            InputManager.Player.Interact.performed -= OnInteractPerform;
        }

        private void Update()
        {
            DetectItem();
        }
        #endregion

        private void DetectItem() 
        {
            bool wasInteract = canInteract;
            //canInteract = Physics.SphereCast(transform.position, radius, cameraTransform.forward, out hit, distance)
            //        && (1 << hit.collider.gameObject.layer) == layerMask.value
            //        && !Physics.CheckSphere(transform.position, radius, wallMask.value);

            canInteract = Physics.Raycast(origin.position, cameraTransform.forward, out hit, distance)
                        && (1 << hit.collider.gameObject.layer) == layerMask.value
                        && (1 << hit.collider.gameObject.layer) != wallMask.value;

            if (wasInteract == canInteract) return;
            if (canInteract)
            {
                 itemDetected = hit.collider.gameObject;
                 onItemDetected?.Invoke();
            }
            else
            {
                onItemUndetected?.Invoke();
            }
            
        }

        private void ItemDetected()
        {
            textUI.SetActive(true);
            canInteract = true;
        }
        private void ItemUndetected()
        {
            textUI.SetActive(false);
            canInteract = false;
        }

        private void OnInteractPerform(InputAction.CallbackContext context)
        {
            if (!canInteract) return;
            Item item = itemDetected.GetComponent<Item>();           
            if (item == null) return;
            GlobalEventManager.CastEvent(GlobalEventIndex.AddItemToInventory, GlobalEventArgsFactory.AddItemToInventoryFactory(itemDetected));
            playerInventory.AddItem(item.ItemObj, item.Quantity, true);
            itemDetected.SetActive(false);
        }


        private void OnDrawGizmos()
        {
            Vector3 pos = origin.position;
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(pos, radius);
            //Gizmos.DrawWireSphere(pos + cameraTransform.forward * distance, radius);
            Gizmos.DrawRay(pos, cameraTransform.forward * distance);
        }
    }
}


/*
private void OnTriggerEnter(Collider other)
{
    if (myCollider == null) return;
    if ((1<< other.gameObject.layer) != layerMask.value) return;
    otherObjs.Add(other);
    textUI.SetActive(true);
}

private void OnTriggerExit(Collider other)
{
    otherObjs.Remove(other);
    if (!IsEmptyItemList()) return;
    textUI.SetActive(false);
}
*/