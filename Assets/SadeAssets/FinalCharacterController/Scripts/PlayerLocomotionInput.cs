using System.Collections;
using System.Collections.Generic;
using SadeAssets.FinalCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    #region Class Variables
    [SerializeField] private bool holdToSprint = true;
    
    public bool SprintToggledOn { get; private set; }

   public PlayerControls PlayerControls {get; private set; }
   public Vector2 MovementInput {get; private set; }
   public Vector2 LookInput {get; private set; }
   public bool JumpPressed { get; private set; }


   public bool AttackPressed { get; private set; }

   #endregion

   #region Startup

   private void OnEnable()
   {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
   }
   



   private void OnDisable()
   {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
   }

    #endregion

    #region Late Update Logic

    private void LateUpdate()
    {
        JumpPressed = false;
        AttackPressed= false;
    }
    #endregion

    #region Input Callbacks
    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        print(MovementInput);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnToggleSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintToggledOn = holdToSprint || !SprintToggledOn;
        }
        else if (context.canceled)
        {
            SprintToggledOn = !holdToSprint && SprintToggledOn;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)     // We don't do anything if space bar is not held down
        return;

        JumpPressed = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        AttackPressed = true;    
    }


    #endregion
}
