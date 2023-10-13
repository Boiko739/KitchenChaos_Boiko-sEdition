using System;
using Unity.VisualScripting;
using UnityEngine;

namespace KitchenChaos
{
    public class GameInput : MonoBehaviour
    {
        public static GameInput Instance { get; private set; }

        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;

        private PlayerInputActions playerInputActions;
        private void Awake()
        {
            Instance = this;

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

        private void PausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractAlternatePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            return inputVector.normalized;
        }
    }
}