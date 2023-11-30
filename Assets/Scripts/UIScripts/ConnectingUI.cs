using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    void Start()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame += KitchenGameMultiplayerOnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayerOnFailedToJoinGame;

        gameObject.SetActive(false);
    }

    private void KitchenGameMultiplayerOnFailedToJoinGame(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void KitchenGameMultiplayerOnTryingToJoinGame(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame -= KitchenGameMultiplayerOnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayerOnFailedToJoinGame;
    }
}
