using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    
    [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idle;
   public enum PlayerMovementState
   {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Sprint = 3,
        Jump = 4,
        Fall = 5,
        Attack = 6,

   }

}
