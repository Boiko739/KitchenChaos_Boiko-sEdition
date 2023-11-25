using KitchenChaos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMultiplayerUI : MonoBehaviour
{  
    void Start()
    {
        GameManager.Instance.OnMultiplayerGamePaused += GameManagerOnMultiplayerGamePaused;
        GameManager.Instance.OnMultiplayerGameUnpaused += GameManagerOnMultiplayerGameUnpaused;

        gameObject.SetActive(false);
    }

    private void GameManagerOnMultiplayerGameUnpaused(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void GameManagerOnMultiplayerGamePaused(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }
}