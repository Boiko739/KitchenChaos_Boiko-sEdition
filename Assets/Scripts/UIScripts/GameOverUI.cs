using OtherScripts;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipesDeliveredText;
        [SerializeField] private Button mainMenuButton;

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += GameManager_OnStateChanged;

            mainMenuButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                Loader.Load(Loader.SceneName.MainMenuScene);
            });

            gameObject.SetActive(false);
        }

        private void GameManager_OnStateChanged(object sender, System.EventArgs e)
        {
            if (GameManager.Instance.IsGameOver())
            {
                gameObject.SetActive(true);
                recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulDeliveriesAmount.ToString();
            }
        }
    }
}