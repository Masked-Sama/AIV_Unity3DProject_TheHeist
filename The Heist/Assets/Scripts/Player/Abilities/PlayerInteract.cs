using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
namespace Player
{
    public class PlayerInteract : PlayerAbilityBase
    {
        
        [SerializeField]
        private GameObject textUI;
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private Collider myCollider;


        private List<Collider> otherObjs = new List<Collider>();
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

        private void OnEnable()
        {
            InputManager.Player.Interact.performed += OnInteractPerform;
        }
        private void OnDisable()
        {
            InputManager.Player.Interact.performed -= OnInteractPerform;
        }

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

        private bool IsEmptyItemList()
        {
            return otherObjs.Count <= 0;
        }

        private void OnInteractPerform(InputAction.CallbackContext context)
        {
            if (IsEmptyItemList()) return;
            GlobalEventManager.CastEvent(GlobalEventIndex.AddItemToInventory, GlobalEventArgsFactory.AddItemToInventoryFactory(otherObjs[0].gameObject));
        }        
    }
}