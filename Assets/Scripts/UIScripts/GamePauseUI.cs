using KitchenChaos;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button optionsButton;

        private void Awake()
        {
            AddingListenersForButtons();
        }

        private void AddingListenersForButtons()
        {
            resumeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.TogglePauseGame();
            });

            mainMenuButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.SceneName.MainMenuScene);
            });

            optionsButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                OptionsUI.Instance.Show(Show);
            });
        }

        private void Start()
        {
            GameManager.Instance.OnLocalGamePaused += GameManagerOnLocalGamePaused;
            GameManager.Instance.OnLocalGameUnpaused += GameManagerOnLocalGameUnpaused;

            gameObject.SetActive(false);
        }

        private void Show()
        {
            gameObject.SetActive(true);
            resumeButton.Select();
        }

        private void GameManagerOnLocalGamePaused(object sender, EventArgs e)
        {
            Show();
        }

        private void GameManagerOnLocalGameUnpaused(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }
    }
}