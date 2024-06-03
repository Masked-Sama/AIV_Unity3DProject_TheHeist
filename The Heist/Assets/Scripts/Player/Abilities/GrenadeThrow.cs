using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class GrenadeThrow : PlayerAbilityBase {
        
        [Header("Variables References")] 
        [SerializeField]
        private Transform camera;
        [SerializeField]
        private Transform attackPoint;
    
        [Header("Settings")] 
        [SerializeField] 
        private float throwCooldown;
    
        [Header("Throwing")] 
        [SerializeField] 
        private KeyCode throwKey = KeyCode.G;
        [SerializeField] 
        private float throwForce;
        [SerializeField] 
        private float throwUpwardForce;
        
        private bool readyToThrow;
        private float currentThrowCD;
    
        #region Mono
        
        private void Update()
        {
            currentThrowCD -= Time.deltaTime;
            if(currentThrowCD > 0) return;
            if(!readyToThrow) return;
            if(Input.GetKeyDown(throwKey)){}
                //Throw();
        }
    
        #endregion
        
        private void OnEnable()
        {
            readyToThrow = true;
            InputManager.Player.Jump.performed += OnThrowPerformed;
        }
    
        private void OnThrowPerformed(InputAction.CallbackContext context) {
            readyToThrow = false;
            
            Debug.Log("Lancio la granata");
            
            //Get From Pool
            // IGrenade grenadeComponent = Pooler.Istance.GetPooledObject(granadeType[0]).GetComponent<IGrenade>();
            // if(grenadeComponent == null) return;
            // grenadeComponent.Throw(camera, attackPoint, throwForce, throwUpwardForce);
            // currentThrowCD = throwCooldown;
        }
    
        public override void OnInputDisabled()
        {
            isPrevented = true;
        }
    
        public override void OnInputEnabled()
        {
            isPrevented = false;
        }
    
        public override void StopAbility() { }
    }

}
