using System.Collections;
using System.Collections.Generic;
using SadeAssets.FinalCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionsInput : MonoBehaviour //PlayerLocomotionInput.IPlayerActionMapActions
{
   #region Class Variables

   public bool AttackPressed { get; private set; }
   

   #endregion


   #region Startup

   /*private void OnEnable()
   {
        if (PlayerInputManager.instance?.PlayerControls == null)
        {
            Debug.LogError("Player controls is not initialized - cannot enable");
            return;
        }

        PlayerInputManager.instance.PlayerControls.PlayerActionMap.Enable();
        PlayerInputManager.instance.PlayerControls.PlayerActionMap.SetCallbacks(this);
   }*/




    #endregion



    #region Late Update Logic

    private void LateUpdate()
    {
        AttackPressed = false;
    }

    #endregion



    #region Input Callbacks
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        AttackPressed = true;    
    }

    #endregion
}
