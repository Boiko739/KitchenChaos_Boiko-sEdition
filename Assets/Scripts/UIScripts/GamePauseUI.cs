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
                OptionsUI.Instance.gameObject.SetActive(true);
            });
        }

        private void Start()
        {
            GameManager.Instance.OnGamePaused += GameManagerOnGamePaused;
            GameManager.Instance.OnGameUnpaused += GameManagerOnGameUnpaused;

            gameObject.SetActive(false);
        }

        private void GameManagerOnGamePaused(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
        }

        private void GameManagerOnGameUnpaused(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }
    }
}