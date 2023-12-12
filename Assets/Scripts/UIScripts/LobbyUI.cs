using KitchenChaos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.SceneName.MainMenuScene);
        });

        createLobbyButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.CrateLobby("Lobby name", false);
        });

        quickJoinButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.QuickJoin();
        });
    }
}
