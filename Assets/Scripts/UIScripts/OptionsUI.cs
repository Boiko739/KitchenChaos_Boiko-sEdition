using KitchenChaos;
using System;
using TMPro;
using UnityEngine;
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
        }

        private void Start()
        {
            GameManager.Instance.OnGameUnpaused += GameManagerOnGameUnpaused;

            UpdateVisual();

            gameObject.SetActive(false);
        }

        private void GameManagerOnGameUnpaused(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void UpdateVisual()
        {
            soundEffectsText.text = $"SOUND EFFECTS: {Math.Round(SoundManager.Instance.Volume * 10f)}";
            musicText.text = $"MUSIC: {Math.Round(MusicManager.Instance.Volume * 10f)}";
        }
    }
}