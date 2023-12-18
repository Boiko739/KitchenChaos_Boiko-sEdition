using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        createPrivateButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.CrateLobby(GetLobbyText(), true);
        });

        createPublicButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.CrateLobby(GetLobbyText(), false);
        });

        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private string GetLobbyText()
    {
        return lobbyNameInputField.text == "" ? "Lobby Name" : lobbyNameInputField.text;
    }
}
