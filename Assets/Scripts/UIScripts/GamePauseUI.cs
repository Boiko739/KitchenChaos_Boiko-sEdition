using OtherScripts;
using System;
using Unity.Netcode;
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
                NetworkManager.Singleton.Shutdown();
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
            GameManager.Instance.OnLocalGamePaused += GameManager_OnLocalGamePaused;
            GameManager.Instance.OnLocalGameUnpaused += GameManager_OnLocalGameUnpaused;

            gameObject.SetActive(false);
        }

        private void Show()
        {
            gameObject.SetActive(true);
            resumeButton.Select();
        }

        private void GameManager_OnLocalGamePaused(object sender, EventArgs e)
        {
            Show();
        }

        private void GameManager_OnLocalGameUnpaused(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }
    }
}