using System;
using UnityEngine;

namespace Data.Scripts
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float _locomotionBlendSpeed = 0.2f;
        
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        
        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");
        private static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
        
        private Vector3 _currentBlendInput = Vector3.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
                
            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, _locomotionBlendSpeed * Time.deltaTime);
            
            animator.SetFloat(inputXHash, _currentBlendInput.x);
            animator.SetFloat(inputYHash, _currentBlendInput.y);
            animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
        }
    }
}
