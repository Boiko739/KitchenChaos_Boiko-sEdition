using OtherScripts;
using OtherScripts;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button createLobbyButton;
        [SerializeField] private Button quickJoinButton;
        [SerializeField] private Button joinCodeButton;
        [SerializeField] private LobbyCreateUI lobbyCreateUI;
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private Transform lobbyListContainer;
        [SerializeField] private Transform lobbyTemplate;

        private void Awake()
        {
            mainMenuButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.LeaveLobby();
                Loader.Load(Loader.SceneName.MainMenuScene);
            });

            createLobbyButton.onClick.AddListener(() =>
            {
                lobbyCreateUI.gameObject.SetActive(true);
            });

            quickJoinButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.QuickJoin();
            });

            joinCodeButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.JoinWithCode(codeInputField.text);
            });

            lobbyTemplate.gameObject.SetActive(false);
        }

        private void Start()
        {
            playerNameInputField.text = KitchenGameMultiplayer.Instance.PlayerName;
            playerNameInputField.onValueChanged.AddListener((newText) =>
            {
                KitchenGameMultiplayer.Instance.PlayerName = newText;
            });

            KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
            UpdateLobbyList(new List<Lobby>());
        }

        private void KitchenGameLobby_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e)
        {
            UpdateLobbyList(e.lobbyList);
        }

        private void UpdateLobbyList(List<Lobby> lobbyList)
        {
            foreach (Transform child in lobbyListContainer)
            {
                if (child != lobbyTemplate)
                {
                    Destroy(child.gameObject);
                }
            }

            foreach (Lobby lobby in lobbyList)
            {
                Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyListContainer);
                lobbyTransform.gameObject.SetActive(true);
                lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
            }
        }

        private void OnDestroy()
        {
            KitchenGameLobby.Instance.OnLobbyListChanged -= KitchenGameLobby_OnLobbyListChanged;
        }
    }
}