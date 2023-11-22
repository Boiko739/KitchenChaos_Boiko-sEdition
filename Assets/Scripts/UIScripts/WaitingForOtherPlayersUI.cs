using KitchenChaos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManagerOnLocalPlayerReadyChanged;
        GameManager.Instance.OnGameStateChanged += GameManagerOnStateChanged;

        gameObject.SetActive(false);
    }

    private void GameManagerOnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStart())
        {
            gameObject.SetActive(false);
        }
    }

    private void GameManagerOnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsLocalPlayerReady)
        {
            gameObject.SetActive(true);
        }
    }
}
