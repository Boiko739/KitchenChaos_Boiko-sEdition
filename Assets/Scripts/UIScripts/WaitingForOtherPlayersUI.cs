using KitchenChaos;
using UnityEngine;

namespace MyUIs
{
    public class WaitingForOtherPlayersUI : MonoBehaviour
    {
        void Start()
        {
            GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
            GameManager.Instance.OnGameStateChanged += GameManager_OnStateChanged;

            gameObject.SetActive(false);
        }

        private void GameManager_OnStateChanged(object sender, System.EventArgs e)
        {
            if (GameManager.Instance.IsCountdownToStart())
            {
                gameObject.SetActive(false);
            }
        }

        private void GameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
        {
            if (GameManager.Instance.IsLocalPlayerReady)
            {
                gameObject.SetActive(true);
            }
        }
    }
}