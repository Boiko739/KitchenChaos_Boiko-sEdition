using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayer_OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
        gameObject.SetActive(false);
    }

    private void KitchenGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to find the Lobby!");
    }

    private void KitchenGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("No available Lobby found!");
    }

    private void KitchenGameLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining Lobby...");
    }

    private void KitchenGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create a Lobby!");
    }

    private void KitchenGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating a Lobby...");
    }

    private void KitchenGameMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        ShowMessage(NetworkManager.Singleton.DisconnectReason == "" ?
            "Failed to connect" : NetworkManager.Singleton.DisconnectReason);
    }

    private void ShowMessage(string message)
    {
        gameObject.SetActive(true);
        messageText.text = message;
    }

    void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayer_OnFailedToJoinGame;
    }
}
