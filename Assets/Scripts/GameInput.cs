using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

   private const string PLAYER_KEY_BINDINGS = "InputBinding";

    public enum Binding {
        Move_Up,
        Move_Left,
        Move_Down,
        Move_Right,
        Interact,
        Interact_Alt,
        Pause,
        GamePad_Interact,
        GamePad_InteractAlt,
        GamePad_Pause
    }
   public static GameInput Instance { get; private set; }
   public event EventHandler OnInteract;
   public event EventHandler OnInteractAlternate;
   public event EventHandler OnGamePaused;
   private PlayerInputActions playerInputActions;

   private void Awake() {
      Instance = this;
      playerInputActions = new PlayerInputActions();
      if(PlayerPrefs.HasKey(PLAYER_KEY_BINDINGS)) {
         playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_KEY_BINDINGS));
      }
      playerInputActions.Player.Enable();
      playerInputActions.Player.Interact.performed += Interact_Performed;
      playerInputActions.Player.InteractAlternate.performed += InteractAlternate_Performed;
      playerInputActions.Player.Pause.performed += OnGamePaused_Performed;
   }

   private void OnGamePaused_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
      OnGamePaused?.Invoke(this,EventArgs.Empty);
   }

   private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
      OnInteract?.Invoke(this, EventArgs.Empty);
   }

    private void InteractAlternate_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
      OnInteractAlternate?.Invoke(this, EventArgs.Empty);
   }

   private void OnDestroy() {
    playerInputActions.Player.Interact.performed -= Interact_Performed;
    playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_Performed;
    playerInputActions.Player.Pause.performed -= OnGamePaused_Performed; 
    playerInputActions.Dispose();     
   }

   public Vector2 GetMovementVectorNormalized() {
    Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
    inputVector = inputVector.normalized;
    return inputVector;
   }

   public string GetBindingText(Binding binding) {

        switch(binding) {
            case Binding.Move_Up:
               return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
               return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
               return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
               return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
               return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Alt:
               return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
               return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.GamePad_Interact:
               return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
            case Binding.GamePad_InteractAlt:
               return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
            case Binding.GamePad_Pause:
               return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
            default:
               return string.Empty;
        }
   }

   public void RebindBinding(Binding binding, Action OnActionRebound) {
      playerInputActions.Player.Disable();
      InputAction inputAction;
      int bindingIndex;
      
      switch(binding) {
         default:
         case Binding.Move_Up:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 1;
         break;
         case Binding.Move_Down:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 2;
         break;
         case Binding.Move_Left:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 3;
         break;
         case Binding.Move_Right:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 4;
         break;
         case Binding.Interact:
            inputAction = playerInputActions.Player.Interact;
            bindingIndex = 0;
         break;
         case Binding.Interact_Alt:
            inputAction = playerInputActions.Player.InteractAlternate;
            bindingIndex = 0;
         break;
         case Binding.Pause:
            inputAction = playerInputActions.Player.Pause;
            bindingIndex = 0;
         break;
         case Binding.GamePad_Interact:
            inputAction = playerInputActions.Player.Interact;
            bindingIndex = 1;
         break;
         case Binding.GamePad_InteractAlt:
            inputAction = playerInputActions.Player.InteractAlternate;
            bindingIndex = 1;
         break;
         case Binding.GamePad_Pause:
            inputAction = playerInputActions.Player.Pause;
            bindingIndex = 1;
         break;
      }

      inputAction.PerformInteractiveRebinding(bindingIndex).
         OnComplete(callback => {
            callback.Dispose();
            playerInputActions.Player.Enable();
            OnActionRebound?.Invoke();
            PlayerPrefs.SetString(PLAYER_KEY_BINDINGS,playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
         })
         .Start();
   }
}
