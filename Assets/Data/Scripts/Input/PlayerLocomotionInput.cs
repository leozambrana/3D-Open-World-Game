using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Data.Scripts.Input.PlayerInputManager;

namespace Data.Scripts
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput: MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        #region  Class Variables

        [SerializeField] private bool holdToSprint = true;
        
        public bool SpringToggleOn { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }
        
        #endregion
        
        #region Start
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.Log("PlayerInputManager.OnEnable: PlayerControls is  - CANNOT ENABLE");
                return;
            }
            
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.Log("PlayerInputManager.OnEnable: PlayerControls is null- CANNOT DISABLE");
                return;
            }
            
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }
        #endregion

        #region Late Update

        private void LateUpdate()
        {
            JumpPressed = false;
        }

        #endregion
      
        #region Input Callbacks
        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnToggleSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SpringToggleOn = holdToSprint || !SpringToggleOn;
            } else if (context.canceled)
            {
                SpringToggleOn = !holdToSprint && SpringToggleOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            JumpPressed = true;
        }
        #endregion
    }
}
