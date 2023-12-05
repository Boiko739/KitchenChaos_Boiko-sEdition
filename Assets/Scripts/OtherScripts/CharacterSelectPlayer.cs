using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private PlayerVisual playerVisual;

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayerOnPlayerDataNetworkListChanged;

        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReadyOnReadyChanged;

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
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(
                KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex).clientId));

            playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerIndex));
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    private void KitchenGameMultiplayerOnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }
}
