using OtherScripts;
using UnityEngine;

namespace MyUIs
{
    public class ConnectingUI : MonoBehaviour
    {
        void Start()
        {
            KitchenGameMultiplayer.Instance.OnTryingToJoinGame += KitchenGameMultiplayer_OnTryingToJoinGame;
            KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayer_OnFailedToJoinGame;

            gameObject.SetActive(false);
        }

        private void KitchenGameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void KitchenGameMultiplayer_OnTryingToJoinGame(object sender, System.EventArgs e)
        {
            gameObject.SetActive(true);
        }

        void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnTryingToJoinGame -= KitchenGameMultiplayer_OnTryingToJoinGame;
            KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayer_OnFailedToJoinGame;
        }
    }
}