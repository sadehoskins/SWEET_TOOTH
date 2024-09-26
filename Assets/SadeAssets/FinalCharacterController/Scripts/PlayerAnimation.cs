using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
     [SerializeField] private Animator _animator;
     [SerializeField] private float locomotionBlendSpeed = 0.02f;

    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;

    private static int inputXHash = Animator.StringToHash("inputX");
    private static int inputYHash = Animator.StringToHash("inputY");

    private static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
    private static int isGroundedHash = Animator.StringToHash("isGrounded");
    private static int isFallingHash = Animator.StringToHash("isFalling");
    private static int isJumpingHash = Animator.StringToHash("isJumping");

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
        bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idle;
        bool isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Run;
        bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprint;
        bool isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jump;
        bool isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Fall;
        bool isGrounded = _playerState.InGroundedState();

        Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

        _animator.SetBool(isGroundedHash, isGrounded);
        _animator.SetBool(isFallingHash, isFalling);
        _animator.SetBool(isJumpingHash, isJumping);
        _animator.SetFloat(inputXHash, inputTarget.x);
        _animator.SetFloat(inputYHash, inputTarget.y);
        _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
    }

}
