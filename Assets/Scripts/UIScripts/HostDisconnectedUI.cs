using OtherScripts;
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class HostDisconnectedUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipesDeliveredText;
        [SerializeField] private Button mainMenuButton;

        void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

            mainMenuButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                Loader.Load(Loader.SceneName.MainMenuScene);
            });

            gameObject.SetActive(false);
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if (clientId == NetworkManager.ServerClientId)
            {
                //Server shut down
                gameObject.SetActive(true);
                recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulDeliveriesAmount.ToString() ?? "0";
            }
        }

        private void OnDestroy()
        {
            try
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
            }
            catch(Exception e)
            {
                print(e);
            }
        }
    }
}