using OtherScripts;
using System;
using UnityEngine;

namespace MyUIs
{
    public class PauseMultiplayerUI : MonoBehaviour
    {
        void Start()
        {
            GameManager.Instance.OnMultiplayerGamePaused += GameManager_OnMultiplayerGamePaused;
            GameManager.Instance.OnMultiplayerGameUnpaused += GameManager_OnMultiplayerGameUnpaused;

            gameObject.SetActive(false);
        }

        private void GameManager_OnMultiplayerGameUnpaused(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void GameManager_OnMultiplayerGamePaused(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
        }
    }
}