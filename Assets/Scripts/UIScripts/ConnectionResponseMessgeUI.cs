using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessgeUI : MonoBehaviour
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
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayerOnFailedToJoinGame;

        gameObject.SetActive(false);
    }

    private void KitchenGameMultiplayerOnFailedToJoinGame(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if (messageText.text == "")
        {
            messageText.text = "Failed to connect";
        }
    }

    void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayerOnFailedToJoinGame;
    }
}
