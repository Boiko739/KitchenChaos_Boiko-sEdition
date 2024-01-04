using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace OtherScripts
{
    public class CharacterSelectPlayer : MonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private GameObject readyGameObject;
        [SerializeField] private PlayerVisual playerVisual;
        [SerializeField] private Button kickButton;
        [SerializeField] private TextMeshPro playerNameText;

        private void Awake()
        {
            kickButton.onClick.AddListener(() =>
            {
                PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
                KitchenGameLobby.Instance.KickPlayer(playerData.playerId.ToString());
                KitchenGameMultiplayer.Instance.KickPlayer(playerData.clientId);
            });
        }

        private void Start()
        {
            KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;

            CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReadyOnReadyChanged;
            kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
            UpdatePlayer();
        }

        private void CharacterSelectReadyOnReadyChanged(object sender, System.EventArgs e)
        {
            UpdatePlayer();
        }

        private void UpdatePlayer()
        {
            if (KitchenGameMultiplayer.Instance.IsPlayerConnected(playerIndex))
            {
                gameObject.SetActive(true);
                PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);

                readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));

                playerNameText.text = playerData.playerName.ToString();

                playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void KitchenGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
        {
            UpdatePlayer();
        }

        private void OnDestroy()
        {
            KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;
        }
    }
}