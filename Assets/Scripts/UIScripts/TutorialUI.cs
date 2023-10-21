using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KitchenChaos
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI keyMoveUpText;
        [SerializeField] private TextMeshProUGUI keyMoveDownText;
        [SerializeField] private TextMeshProUGUI keyMoveRightText;
        [SerializeField] private TextMeshProUGUI keyMoveLeftText;
        [SerializeField] private TextMeshProUGUI keyboardInteractText;
        [SerializeField] private TextMeshProUGUI keyboardInteractAlternateText;
        [SerializeField] private TextMeshProUGUI keyboardPauseText;

        [SerializeField] private TextMeshProUGUI gamepadInteractText;
        [SerializeField] private TextMeshProUGUI gamepadInteractAlternateText;
        [SerializeField] private TextMeshProUGUI gamepadPauseText;

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;

            UpdateVisual();
            GameInput.Instance.OnBindingRebind += GameInputOnBindingRebind;
            gameObject.SetActive(GameManager.IsFirstGame);
        }

        private void GameManagerOnStateChanged(object sender, System.EventArgs e)
        {
            if (!GameManager.Instance.IsGameWaitingToStart())
            {
                gameObject.SetActive(false);
            }
        }

        private void GameInputOnBindingRebind(object sender, System.EventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
            keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
            keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
            keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
            keyboardInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
            keyboardInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
            keyboardPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

            gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadInteract);
            gamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadInteractAlternate);
            gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadPause);
        }
    }
}