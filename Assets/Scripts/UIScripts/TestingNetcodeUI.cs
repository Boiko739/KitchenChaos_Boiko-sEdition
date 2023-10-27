using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : NetworkBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            print("HOST!");
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false);
        });

        startClientButton.onClick.AddListener(() =>
        {
            print("CLIENT!");
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
        });
    }
}
