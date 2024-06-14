using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Player
{
    public class PlayerInteract : PlayerAbilityBase
    {
        [SerializeField]
        private Transform cameraPosition;
        [SerializeField]
        private float distance;

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

            //canInteract = Physics.SphereCast(transform.position, radius, cameraPosition.forward, out hit, distance)
            //        && (1 << hit.collider.gameObject.layer) == layerMask.value
            //        && !Physics.CheckSphere(transform.position, radius, wallMask.value);

            canInteract = Physics.Raycast(cameraPosition.position, cameraPosition.forward, out hit, distance)
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
            WeaponData weapon;
            switch (item.ItemObj.ItemType)
            {
                case ItemType.FirstWeapon:
                    weapon = (FirstWeaponData)item.ItemObj;
                    playerController.OnChangeWeapon?.Invoke(weapon);
                    break;
                case ItemType.SecondWeapon:
                    weapon = (SecondWeaponData)item.ItemObj;
                    playerController.OnChangeWeapon?.Invoke(weapon);
                    break;
                case ItemType.ThrowableWeapon:
                    ThrowableData throwable = (ThrowableData)item.ItemObj;
                    break;
                case ItemType.Consumable:
                    //  TODO
                    break;
                default:
                    return;

            }            
            GlobalEventManager.CastEvent(GlobalEventIndex.AddItemToInventory, GlobalEventArgsFactory.AddItemToInventoryFactory(itemDetected));
            playerInventory.AddItem(item.ItemObj, item.Quantity, true);
            itemDetected.SetActive(false);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            //Gizmos.DrawWireSphere(pos, radius);
            //Gizmos.DrawWireSphere(pos + cameraPosition.forward * distance, radius);

            Gizmos.DrawRay(cameraPosition.position, cameraPosition.forward * distance);
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