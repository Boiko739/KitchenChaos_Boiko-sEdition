using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayerOnPlayerDataNetworkListChanged;

        PlayerUpdate();
    }

    private void PlayerUpdate()
    {
        gameObject.SetActive(KitchenGameMultiplayer.Instance.IsPlayerConnected(playerIndex));
    }

    private void KitchenGameMultiplayerOnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        PlayerUpdate();
    }
}
