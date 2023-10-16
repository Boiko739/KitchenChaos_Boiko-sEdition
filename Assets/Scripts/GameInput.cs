using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenChaos
{
    public class GameInput : MonoBehaviour
    {
        private const string PLAYER_PREFS_BINDINGS = "InputBindings";

        public static GameInput Instance { get; private set; }

        public enum Binding
        {
            MoveUp,
            MoveDown,
            MoveRight,
            MoveLeft,
            Interact,
            InteractAlternate,
            Pause
        }

        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            Instance = this;

            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            {
                playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
            }

            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();

            playerInputActions.Player.Interact.performed += InteractPerformed;
            playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
            playerInputActions.Player.Pause.performed += PausePerformed;
        }

        private void OnDestroy()
        {
            playerInputActions.Player.Interact.performed -= InteractPerformed;
            playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePerformed;
            playerInputActions.Player.Pause.performed -= PausePerformed;

            playerInputActions.Dispose();
        }

        private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractAlternatePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
        }

        private void PausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            return inputVector.normalized;
        }

        public string GetBindingText(Binding binding)
        {
            switch (binding)
            {
                case Binding.MoveUp:
                    return playerInputActions.Player.Move.bindings[1].ToDisplayString();
                case Binding.MoveDown:
                    return playerInputActions.Player.Move.bindings[2].ToDisplayString();
                case Binding.MoveRight:
                    return playerInputActions.Player.Move.bindings[3].ToDisplayString();
                case Binding.MoveLeft:
                    return playerInputActions.Player.Move.bindings[4].ToDisplayString();
                case Binding.Interact:
                    return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
                case Binding.InteractAlternate:
                    return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
                case Binding.Pause:
                    return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
                default:
                    return string.Empty;
            }
        }

        public void RebindBinding(Binding binding, Action onActionRebound)
        {
            playerInputActions.Player.Disable();


            InputAction inputAction;
            int bindingIndex;

            switch (binding)
            {
                default:
                //case Binding.MoveUp:
                    inputAction = playerInputActions.Player.Move;
                    bindingIndex = 1;
                    break;
                case Binding.MoveDown:
                    inputAction = playerInputActions.Player.Move;
                    bindingIndex = 2;
                    break;
                case Binding.MoveRight:
                    inputAction = playerInputActions.Player.Move;
                    bindingIndex = 3;
                    break;
                case Binding.MoveLeft:
                    inputAction = playerInputActions.Player.Move;
                    bindingIndex = 4;
                    break;
                case Binding.Interact:
                    inputAction = playerInputActions.Player.Interact;
                    bindingIndex = 0;
                    break;
                case Binding.InteractAlternate:
                    inputAction = playerInputActions.Player.InteractAlternate;
                    bindingIndex = 0;
                    break;
                case Binding.Pause:
                    inputAction = playerInputActions.Player.Pause;
                    bindingIndex = 0;
                    break;
            }

            inputAction.PerformInteractiveRebinding(bindingIndex)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    playerInputActions.Player.Enable();
                    onActionRebound();
                    PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();
                })
                .Start();
        }
    }
}