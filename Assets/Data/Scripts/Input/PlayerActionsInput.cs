using UnityEngine;
using UnityEngine.InputSystem;

namespace Data.Scripts.Input
{
    public class PlayerActionsInput : MonoBehaviour, PlayerControls.IPlayerActionMapActions
    {
        public bool AttackPressed { get; private set; }
        public bool GatherPressed { get; private set; }

        #region Start
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.Log("PlayerInputManager.OnEnable: PlayerControls is  - CANNOT ENABLE");
                return;
            }
            
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.Log("PlayerInputManager.OnEnable: PlayerControls is null- CANNOT DISABLE");
                return;
            }
            
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.RemoveCallbacks(this);
        }
        #endregion
        private void LateUpdate()
        {
            AttackPressed = false;
            GatherPressed = false;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            print("enter here" +  context.ReadValue<Vector2>());
            if(!context.performed) return;
            AttackPressed  = true;
        }

        public void OnGather(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
            GatherPressed = true;
        }
    }
}
