using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEditor;
using UnityEditor.Callbacks;


//using System.Numerics;
using UnityEngine;

[DefaultExecutionOrder(-1)]

public class PlayerController : MonoBehaviour
{
    #region Class Variables

    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;      // C# "_" for _memberField
                                                        // Unity "m_" for m_Memberfield
    [Header("Base Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float sprintAcceleration = 0.5f;
    public float sprintSpeed = 7f;
    public float drag = 0.1f;
    
    public float gravity = 25f;
    public float jumpSpeed = 1.0f;
    public float movingThreshold = 0.01f;

    public Weapon weapon;

    //Weapon test

    public float moveSpeed = 5f;
    public Rigidbody rb;
    Vector2 moveDirection;
    Vector2 mousePosition;
    //weapon test


    [Header("Camera Settings")]
    public float lookSenseH = 0.1f; //look sensetivity
    public float lookSenseV = 0.1f;
    public float lookLimitV = 89f;



    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;
    private Vector2 _cameraRotation = Vector2.zero;
    private Vector2 _playerTargetRotation = Vector2.zero;

    private float _verticalVelocity = 0f;
    
    #endregion


    #region Startup
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


        //Weapon Test
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        //Weapon Test

        if(Input.GetMouseButtonDown(0))
        {
            weapon.Fire();
  
        }

        // Weapon Test
        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Weapon Test
    }          

    /*weapon Test
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        Vector2 aimDirection = mousePositdion - _verticalVelocity;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    }
    Weapon Test*/

    private void UpdateMovementState()
    {
        bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero; //order matters 1
        bool isMovingLaterally = IsMovingLaterally();   //2
        bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally; //3
        bool isGrounded = IsGrounded();

        PlayerMovementState lateralState =  isSprinting ? PlayerMovementState.Sprint :
                                            isMovingLaterally || isMovementInput ? PlayerMovementState.Run : PlayerMovementState.Idle;
        
        _playerState.SetPlayerMovementState(lateralState);

        // Control Airborn State
        if (!isGrounded && _characterController.velocity.y >= 0f)
        {
            _playerState.SetPlayerMovementState(PlayerMovementState.Jump);
        }/*
        else if (!isGrounded && _characterController.velocity.y < 0f)
        {
            _playerState.SetPlayerMovementState(PlayerMovementState.Fall);  //overrides lateral state
        }*/

    }

    private void HandleVerticalMovement()
    {
        bool isGrounded = _playerState.InGroundedState();

        if (isGrounded && _verticalVelocity < 0)
            _verticalVelocity = 0f;

        _verticalVelocity -= gravity * Time.deltaTime;

        if (_playerLocomotionInput.JumpPressed && isGrounded)
        {
            _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity); // From Unity documentation
        }
    }

    private void HandleLateralMovement()
    {
        // Create quick references for curren state
        bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprint;
        bool isGrounded = _playerState.InGroundedState();

        // State dependent acceleration and speed 
        float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
        float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;

        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

        Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
        Vector3 newVelocity = _characterController.velocity + movementDelta;

        // Add drag to player
        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        // Prevents Acceleration being greater than run speed
        newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);
        newVelocity.y += _verticalVelocity;


        // Move character (Unity suggest only calling this once per tick)
        _characterController.Move(newVelocity * Time.deltaTime);
    }

    #endregion

    #region Late Update Logic

    private void LateUpdate()
    {
        _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
    }     

    #endregion

    #region State Checks   

    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.y);
        return lateralVelocity.magnitude > movingThreshold;
    }         

    private bool IsGrounded()
    {
        return _characterController.isGrounded;
    }

    #endregion                   

}
