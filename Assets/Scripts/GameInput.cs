using System;
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
            Pause,
            GamepadInteract,
            GamepadInteractAlternate,
            GamepadPause
        }

        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;
        public event EventHandler OnBindingRebind;

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            Instance = this;

            playerInputActions = new PlayerInputActions();

            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            {
                playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
            }

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

        private void InteractPerformed(InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractAlternatePerformed(InputAction.CallbackContext obj)
        {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
        }

        private void PausePerformed(InputAction.CallbackContext obj)
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
            return binding switch
            {
                Binding.MoveUp => playerInputActions.Player.Move.bindings[1].ToDisplayString(),
                Binding.MoveDown => playerInputActions.Player.Move.bindings[2].ToDisplayString(),
                Binding.MoveRight => playerInputActions.Player.Move.bindings[3].ToDisplayString(),
                Binding.MoveLeft => playerInputActions.Player.Move.bindings[4].ToDisplayString(),
                Binding.Interact => playerInputActions.Player.Interact.bindings[0].ToDisplayString(),
                Binding.InteractAlternate => playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
                Binding.Pause => playerInputActions.Player.Pause.bindings[0].ToDisplayString(),
                Binding.GamepadInteract => playerInputActions.Player.Interact.bindings[1].ToDisplayString(),
                Binding.GamepadInteractAlternate => playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString(),
                Binding.GamepadPause => playerInputActions.Player.Pause.bindings[1].ToDisplayString(),
                _ => string.Empty,
            };
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
                case Binding.GamepadInteract:
                    inputAction = playerInputActions.Player.Interact;
                    bindingIndex = 1;
                    break;
                case Binding.GamepadInteractAlternate:
                    inputAction = playerInputActions.Player.InteractAlternate;
                    bindingIndex = 1;
                    break;
                case Binding.GamepadPause:
                    inputAction = playerInputActions.Player.Pause;
                    bindingIndex = 1;
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

                    OnBindingRebind(this, EventArgs.Empty);
                })
                .Start();
        }
    }
}