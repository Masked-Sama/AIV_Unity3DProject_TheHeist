using UnityEngine;
using UnityEngine.InputSystem;
using System;
using static Codice.Client.Common.Connection.AskCredentialsToUser;
using UnityEditor.UIElements;

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

        //private Action onItemDetected;
        //private Action onItemUndetected;
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
                playerController.OnItemDetected?.Invoke();
            }
            else
            {
                playerController.OnItemUndetected?.Invoke();
            }
        }

        private void ItemDetected()
        {
            if (itemDetected.GetComponent<Item>() != null)
                textUI.GetComponent<UnityEngine.UI.Text>().text =
                    $"{itemDetected.GetComponent<Item>().ItemData.ItemName} - Cost: {itemDetected.GetComponent<Item>().ItemData.Cost}"; //DA CAMBIARE ASSOLUTAMENTE
            else
            {
                textUI.GetComponent<UnityEngine.UI.Text>().text = "Press E to Interact";
            }

            textUI.SetActive(true);
            canInteract = true;
        }

        private void ItemUndetected()
        {
            if (textUI == null) return;
            textUI.SetActive(false);
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
            Item itemComponent = itemDetected.GetComponent<Item>();
            if (itemComponent == null) return;

            if (itemDetected.CompareTag(sellingWeaponTag))
            {
                if (playerController.OnTryToBuyItem == null) return;
                if (playerController.OnTryToBuyItem.Invoke(itemComponent.ItemData.Cost))
                {
                    Debug.Log("C'ho li sordi");
                    GlobalEventManager.CastEvent(GlobalEventIndex.BuyItem,
                        GlobalEventArgsFactory.BuyItemFactory(itemDetected));
                }
                else
                {
                    Debug.Log("Non c'ho li sordi");
                    return;
                }
            }

            WeaponData weapon;
            switch (itemComponent.ItemData.ItemType)
            {
                case ItemType.FirstWeapon:
                    weapon = (FirstWeaponData)itemComponent.ItemData;
                    playerController.OnChangeWeapon?.Invoke(weapon);
                    break;
                case ItemType.SecondWeapon:
                    weapon = (SecondWeaponData)itemComponent.ItemData;
                    playerController.OnChangeWeapon?.Invoke(weapon);
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