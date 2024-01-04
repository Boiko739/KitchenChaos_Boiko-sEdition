using Unity.Netcode;
using UnityEngine;

namespace OtherScripts
{
    public class MainMenuCleanUp : MonoBehaviour
    {
        public void Awake()
        {
            if (NetworkManager.Singleton != null)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }

            if (KitchenGameMultiplayer.Instance != null)
            {
                Destroy(KitchenGameMultiplayer.Instance.gameObject);
            }

            if (KitchenGameLobby.Instance != null)
            {
                Destroy(KitchenGameLobby.Instance.gameObject);
            }
        }
    }
}