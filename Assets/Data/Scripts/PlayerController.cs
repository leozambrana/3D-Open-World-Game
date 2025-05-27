using System;
using UnityEngine;

namespace Data.Scripts
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        #region Class Variables
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;
    
        [Header("Base Movement")]
        public float runAcceleration = 0.25f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 0.5f;
        public float sprintSpeed = 7f;
        public float drag = 0.1f;
        public float movingThreshold = 0.01f;
        
        [Header("Camera Settings")]
        public float lookSenseH = 0.1F;
        public  float lookSenseV = 0.1f;
        public float lookLimitV = 89F;
    
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;
        #endregion
        
        #region StartUp
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();  
            _playerState = GetComponent<PlayerState>();
        }
        #endregion

        #region Update Logic
        private void Update()
        {
            UpdateMovementState();
            HandleLateralMovement();
        }
        
        private void UpdateMovementState()
        {
            // a ordem desses bools importam
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
            bool isMovementLaterally = IsMovementLaterally();
            bool isSprinting = _playerLocomotionInput.SpringToggleOn && isMovementLaterally;
            
            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                isMovementLaterally || isMovementInput ? 
                    PlayerMovementState.Running : 
                    PlayerMovementState.Idling;
            _playerState.SetPlayerMovementState(lateralState);
        }

        private void HandleLateralMovement()
        {
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            
            //esse state depende da aceleracao e velocidade
            float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
            float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;
            
            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;
        
            Vector3 movementDelta = movementDirection * (lateralAcceleration * Time.deltaTime);
            Vector3 newVelocity = _characterController.velocity + movementDelta;
        
            Vector3 currentDrag = newVelocity.normalized * (drag * Time.deltaTime);
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);
        
            // Move character deve ser chamado 1 vez por frame somente
            _characterController.Move(newVelocity * Time.deltaTime);
        }
        #endregion

        #region Late Update Logic
        private void LateUpdate()
        {
            // Rotação Horizontal (Player)
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            
            // Rotação Vertical (Câmera)
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);
            
            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            
            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }
        #endregion
        
        #region State Checks 

        private bool IsMovementLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreshold;
        }
        #endregion
    }
}
