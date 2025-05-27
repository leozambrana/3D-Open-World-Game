using System;
using Unity.VisualScripting;
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
        public float RotationMismatch { get; private set; } = 0f;
        public bool IsRotationToTarget { get; private set; } = false;
    
        [Header("Base Movement")]
        public float runAcceleration = 0.25f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 0.5f;
        public float sprintSpeed = 7f;
        public float drag = 0.1f;
        public float gravity = 9.8f;
        public float jumpSpeed = 1.0f;
        public float movingThreshold = 0.01f;

        [Header("Animations")] 
        public float playerModelRotationSpeed = 10;
        public float rotateToTargetTime = 0.25f;
        
        [Header("Camera Settings")]
        public float lookSenseH = 0.1F;
        public  float lookSenseV = 0.1f;
        public float lookLimitV = 89F;
    
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;
        
        private bool _isRotatingClockwise = false;
        private float _rotatingToTargetTimer = 0f;
        private float _verticalVelocity = 0f;
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
            HandleVerticalMovement();
            HandleLateralMovement();
        }
        
        private void UpdateMovementState()
        {
            // a ordem desses bools importam
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
            bool isMovementLaterally = IsMovementLaterally();
            bool isSprinting = _playerLocomotionInput.SpringToggleOn && isMovementLaterally;
            bool isGrounded = IsGrounded();
            
            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                isMovementLaterally || isMovementInput ? 
                    PlayerMovementState.Running : 
                    PlayerMovementState.Idling;
            _playerState.SetPlayerMovementState(lateralState);

            if (!isGrounded && _characterController.velocity.y > 0f)
            {
                print("entra aqui" +  _characterController.velocity.y);
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
            } else if (!isGrounded && _characterController.velocity.y <= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
            }
        }

        private void HandleLateralMovement()
        {
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerState.IsGroundedState();
            
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
            newVelocity.y += _verticalVelocity;
        
            // Move character deve ser chamado 1 vez por frame somente
            _characterController.Move(newVelocity * Time.deltaTime);
        }

        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.IsGroundedState();

            if (isGrounded && _verticalVelocity < 0f) _verticalVelocity = 0f;
            
            _verticalVelocity -= gravity * Time.deltaTime;

            if (_playerLocomotionInput.JumpPressed && isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity);
            }
        }
        #endregion

        #region Late Update Logic
        private void LateUpdate()
        {
            UpdateCameraRotation();
        }

        private void UpdateCameraRotation()
        {
            // Rotação Horizontal (Player)
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            
            // Rotação Vertical (Câmera)
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);
            
            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;

            float rotationTolerance = 90f;
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            IsRotationToTarget = _rotatingToTargetTimer > 0;

            if (!isIdling)
            {
                RotatePlayerToTarget();
            }
            else if (Mathf.Abs(RotationMismatch) > rotationTolerance || IsRotationToTarget)
            {
                UpdateIdleRotation(rotationTolerance);
            }
            
            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
            
            Vector3 canForwardProjectedXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 crossProduct = Vector3.Cross(transform.forward, canForwardProjectedXZ);
            float sign = Mathf.Sign(Vector3.Dot(crossProduct, transform.up));
            RotationMismatch = sign * Vector3.Angle(transform.forward, canForwardProjectedXZ);
        }

        private void UpdateIdleRotation(float rotationTolerance)
        {
            //inicia uma nova direcao de rotação
            if (Mathf.Abs(RotationMismatch) > rotationTolerance)
            {
                _rotatingToTargetTimer = rotateToTargetTime;
                _isRotatingClockwise = RotationMismatch > rotationTolerance;
            }
            _rotatingToTargetTimer -= Time.deltaTime;  
            
            //roda o player
            if (_isRotatingClockwise && RotationMismatch > 0f || !_isRotatingClockwise && RotationMismatch < 0f)
            {
                RotatePlayerToTarget();
            }
        }

        private void RotatePlayerToTarget()
        {
            Quaternion targetRotationX = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, playerModelRotationSpeed * Time.deltaTime);
        }
        #endregion
        
        #region State Checks 

        private bool IsMovementLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreshold;
        }

        private bool IsGrounded()
        {
            return _characterController.isGrounded;
        }
        #endregion
    }
}
