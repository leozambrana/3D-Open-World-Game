using System;
using Data.Scripts.Input;
using UnityEngine;

namespace Data.Scripts
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float _locomotionBlendSpeed = 0.2f;
        
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerActionsInput _playerActionsInput;
        private PlayerState _playerState;
        private PlayerController _playerController;
        
        //Locomotion
        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");
        private static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
        private static int isGroundedHash = Animator.StringToHash("isGrounded");
        private static int isJumpingHash = Animator.StringToHash("isJumping");
        private static int isFallingHash = Animator.StringToHash("isFalling");
        private static int isIdlingHash = Animator.StringToHash("isIdling");
        
        //Camera rotate
        private static int isRotatingToTargetHash = Animator.StringToHash("isRotatingToTarget");
        private static int rotationMismatchHash = Animator.StringToHash("rotationMismatch");

        //Actions
        private static int isAttackingHash = Animator.StringToHash("isAttacking");
        private static int isGatheringHash = Animator.StringToHash("isGathering");
        
        private Vector3 _currentBlendInput = Vector3.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
            _playerController = GetComponent<PlayerController>();
            _playerActionsInput = GetComponent<PlayerActionsInput>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            bool isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
            bool isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
            bool isGrounded = _playerState.IsGroundedState();
            
            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, _locomotionBlendSpeed * Time.deltaTime);
            
            animator.SetBool(isGroundedHash, isGrounded);
            animator.SetBool(isJumpingHash, isJumping);
            animator.SetBool(isFallingHash, isFalling);
            animator.SetBool(isIdlingHash, isIdling);
            animator.SetBool(isRotatingToTargetHash, _playerController.IsRotationToTarget);
            animator.SetBool(isAttackingHash, _playerActionsInput.AttackPressed);
            animator.SetBool(isGatheringHash, _playerActionsInput.GatherPressed);
            
            animator.SetFloat(inputXHash, _currentBlendInput.x);
            animator.SetFloat(inputYHash, _currentBlendInput.y);
            animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
            animator.SetFloat(rotationMismatchHash, _playerController.RotationMismatch);
        }
    }
}
