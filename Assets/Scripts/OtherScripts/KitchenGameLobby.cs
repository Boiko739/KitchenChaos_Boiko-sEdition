using KitchenChaos;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class KitchenGameLobby : MonoBehaviour
{
    public const int MAX_PLAYERS_AMOUNT = 4;

    private Lobby lobby;
    private Lobby joinedLobby;

    public static KitchenGameLobby Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync(new InitializationOptions().
                SetProfile(UnityEngine.Random.Range(0, 10_000).ToString()));
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CrateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYERS_AMOUNT, new CreateLobbyOptions() { IsPrivate = isPrivate });

            KitchenGameMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.SceneName.CharacterSelectScene);
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }

    public async void QuickJoin()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }
}
