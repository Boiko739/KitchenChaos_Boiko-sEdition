using KitchenChaos;
using OtherScripts;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class TestingLobbyUI : MonoBehaviour
    {
        [SerializeField] private Button createGameButton;
        [SerializeField] private Button joinGameButton;

        private void Awake()
        {
            createGameButton.onClick.AddListener(() =>
            {
                KitchenGameMultiplayer.Instance.StartHost();
                Loader.LoadNetwork(Loader.SceneName.CharacterSelectScene);
            });

            joinGameButton.onClick.AddListener(() =>
            {
                KitchenGameMultiplayer.Instance.StartClient();
            });
        }

    }
}