using KitchenChaos;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MyUIs
{
    public class OptionsUI : MonoBehaviour
    {
        public static OptionsUI Instance { get; private set; }

        [SerializeField] private Button soundEffectsButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private TextMeshProUGUI soundEffectsText;
        [SerializeField] private TextMeshProUGUI musicText;

        [SerializeField] private TextMeshProUGUI moveUpText;
        [SerializeField] private TextMeshProUGUI moveDownText;
        [SerializeField] private TextMeshProUGUI moveRightText;
        [SerializeField] private TextMeshProUGUI moveLeftText;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private TextMeshProUGUI interactAlternateText;
        [SerializeField] private TextMeshProUGUI pauseText;

        [SerializeField] private Button moveUpButton;
        [SerializeField] private Button moveDownButton;
        [SerializeField] private Button moveRightButton;
        [SerializeField] private Button moveLeftButton;
        [SerializeField] private Button interactButton;
        [SerializeField] private Button interactAlternateButton;
        [SerializeField] private Button pauseButton;

        [SerializeField] private Transform pressToRebindKeyTransform;

        private void Awake()
        {
            Instance = this;

            soundEffectsButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.ChangeVolume();
                UpdateVisual();
            });

            musicButton.onClick.AddListener(() =>
            {
                MusicManager.Instance.ChangeVolume();
                UpdateVisual();
            });

            closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });

            moveUpButton.onClick.AddListener(() =>
            {
                RebindBinding(GameInput.Binding.MoveUp);
            });

            moveDownButton.onClick.AddListener(() =>
            {
                RebindBinding(GameInput.Binding.MoveDown);
            });

            moveRightButton.onClick.AddListener(() =>
            {
                RebindBinding(GameInput.Binding.MoveRight);
            });

            moveLeftButton.onClick.AddListener(() =>
            {
                RebindBinding(GameInput.Binding.MoveLeft);
            });

            interactButton.onClick.AddListener(() =>
            {
                RebindBinding(GameInput.Binding.Interact);
            });

            interactAlternateButton.onClick.AddListener(() =>
            {
                RebindBinding(GameInput.Binding.InteractAlternate);
            });

            pauseButton.onClick.AddListener(() =>
            {
                RebindBinding(GameInput.Binding.Pause);
            });
        }

        private void Start()
        {
            GameManager.Instance.OnGameUnpaused += GameManagerOnGameUnpaused;

            UpdateVisual();

            gameObject.SetActive(false);
            pressToRebindKeyTransform.gameObject.SetActive(false);
        }

        private void GameManagerOnGameUnpaused(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void UpdateVisual()
        {
            soundEffectsText.text = $"SOUND EFFECTS: {Math.Round(SoundManager.Instance.Volume * 10f)}";
            musicText.text = $"MUSIC: {Math.Round(MusicManager.Instance.Volume * 10f)}";

            moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
            moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
            moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
            moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
            interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
            interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
            pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        }

        private void RebindBinding(GameInput.Binding binding)
        {
            pressToRebindKeyTransform.gameObject.SetActive(true);
            GameInput.Instance.RebindBinding(binding, () => 
            {
                pressToRebindKeyTransform.gameObject.SetActive(false);
                UpdateVisual();
            });
        }
    }
}