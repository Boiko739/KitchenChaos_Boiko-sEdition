using OtherScripts;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class TestingNetcodeUI : NetworkBehaviour
    {
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;

        private void Awake()
        {
            startHostButton.onClick.AddListener(() =>
            {
                print("HOST!");
                KitchenGameMultiplayer.Instance.StartHost();
                gameObject.SetActive(false);
            });

            startClientButton.onClick.AddListener(() =>
            {
                print("CLIENT!");
                KitchenGameMultiplayer.Instance.StartClient();
                gameObject.SetActive(false);
            });
        }
    }
}