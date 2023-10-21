using KitchenChaos;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(() =>
            {
                Loader.Load(Loader.SceneName.GameScene);
            });

            quitButton.onClick.AddListener(() =>
            {
                GameManager.IsFirstGame = true;
                Application.Quit();
            });

            Time.timeScale = 1f;
        }
    }
}