using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteract : PlayerAbilityBase
    {
        private const string sellingWeaponTag = "SellingWeapon";

        [SerializeField]
        private float distance;

        [SerializeField]
        private GameObject textUI;
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private LayerMask wallMask;

        private GameObject itemDetected;
        private Item itemComponent;
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
            playerController.OnItemUndetected += ItemUndetected;
            playerController.OnItemDetected += ItemDetected;
        }

        private void OnDisable()
        {
            InputManager.Player.Interact.performed -= OnInteractPerform;
        }

        private void Update()
        {
            if (isPrevented) return;
                DetectItem();

        }

        #endregion

        private void DetectItem()
        {
            bool wasInteract = canInteract;
            canInteract = Physics.Raycast(playerController.CameraPositionTransform.position, playerController.CameraPositionTransform.forward, out hit, distance)
                        && (1 << hit.collider.gameObject.layer) == layerMask.value
                        && (1 << hit.collider.gameObject.layer) != wallMask.value;

            if (wasInteract == canInteract) return;
            if (canInteract)
            {
                itemDetected = hit.collider.gameObject;
                itemComponent = itemDetected.GetComponent<Item>();
                playerController.OnItemDetected?.Invoke();
            }
            else
            {
                itemComponent = null;
                playerController.OnItemUndetected?.Invoke();
            }
        }

        private void ItemDetected()
        {
            canInteract = true;
            string message;
            if (itemComponent== null)
            {
                message = "E\n Start Mission";
                GlobalEventManager.CastEvent(GlobalEventIndex.ShowStringInUI, GlobalEventArgsFactory.ShowStringInUIFactory(message,Color.yellow,24));                
                return;
            }
            if (itemDetected.CompareTag(sellingWeaponTag))
            {
                message = $"E\nBuy {itemComponent.ItemData.ItemName} - Cost: {itemComponent.ItemData.Cost}";
                GlobalEventManager.CastEvent(GlobalEventIndex.ShowStringInUI,GlobalEventArgsFactory.ShowStringInUIFactory(message,Color.green, 18));
                return;
            }
            else
            {
                message = $"E\n Pick up {itemComponent.ItemData.ItemName} - Quantity: {itemComponent.Quantity}";
                GlobalEventManager.CastEvent(GlobalEventIndex.ShowStringInUI, GlobalEventArgsFactory.ShowStringInUIFactory(message, Color.green, 18));
                return;
            }
            
        }

        private void ItemUndetected()
        {
            GlobalEventManager.CastEvent(GlobalEventIndex.HideStringInUI, GlobalEventArgsFactory.HideStringInUIFactory());
            canInteract = false;
        }

        private void OnInteractPerform(InputAction.CallbackContext context)
        {
            if (!canInteract) return;
            ChangeScene sceneRef = itemDetected.GetComponent<ChangeScene>();
            if (sceneRef != null)
            {
                sceneRef.ChangeSceneStarter = true;
                return;
            }
            itemComponent = itemDetected.GetComponent<Item>();
            if (itemComponent == null) return;

            if (itemDetected.CompareTag(sellingWeaponTag))
            {
                if (playerController.OnTryToBuyItem == null) return;
                if (playerController.OnTryToBuyItem.Invoke(itemComponent.ItemData.Cost))
                {
                    GlobalEventManager.CastEvent(GlobalEventIndex.BuyItem,
                        GlobalEventArgsFactory.BuyItemFactory(itemDetected));
                }
                else
                {
                    return;
                }
            }

            WeaponData weapon=null;
            switch (itemComponent.ItemData.ItemType)
            {
                case ItemType.FirstWeapon:
                    weapon = (FirstWeaponData)itemComponent.ItemData;
                    break;
                case ItemType.SecondWeapon:
                    weapon = (SecondWeaponData)itemComponent.ItemData;
                    break;
                case ItemType.ThrowableWeapon:
                    ThrowableData throwable = (ThrowableData)itemComponent.ItemData;
                    break;
                case ItemType.Consumable:
                    //  TODO
                    break;
                default:
                    return;
            }


            GlobalEventManager.CastEvent(GlobalEventIndex.AddItemToInventory, GlobalEventArgsFactory.AddItemToInventoryFactory(itemDetected));
            playerController.Inventory.AddItem(itemComponent.ItemData, itemComponent.Quantity);

            if (!itemDetected.CompareTag(sellingWeaponTag))
                itemDetected.SetActive(false);
            if (weapon !=null)
                playerController.OnChangeWeapon?.Invoke(weapon);
        }

        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            //Gizmos.DrawWireSphere(pos, radius);
            //Gizmos.DrawWireSphere(pos + cameraPosition.forward * distance, radius);

            Gizmos.DrawRay(playerController.CameraPositionTransform.position, playerController.CameraPositionTransform.forward * distance);
        }*/
    }
}
