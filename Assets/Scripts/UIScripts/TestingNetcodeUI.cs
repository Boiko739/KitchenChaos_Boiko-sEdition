using KitchenChaos;
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
            GameManager.Instance.StartHost();
            gameObject.SetActive(false);
        });

        startClientButton.onClick.AddListener(() =>
        {
            print("CLIENT!");
            GameManager.Instance.StartClient();
            gameObject.SetActive(false);
        });
    }
}
