using KitchenChaos;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class KitchenGameLobby : MonoBehaviour
{
    public const int MAX_PLAYERS_AMOUNT = 4;

    private Lobby joinedLobby;
    private float heatbeatTimer;
    private float listLobbiesTimer;

    public static KitchenGameLobby Instance { get; private set; }

    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;
    public event EventHandler OnJoinStarted;
    public event EventHandler OnJoinFailed;
    public event EventHandler OnQuickJoinFailed;
    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

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

    private void Update()
    {
        HandleHeartbeat();
        HandlePeriodicListLobbies();
    }

    private void HandlePeriodicListLobbies()
    {
        if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn)
        {
            listLobbiesTimer -= Time.deltaTime;
            if (listLobbiesTimer <= 0)
            {
                float listLobbiesTimerMax = 3f;
                listLobbiesTimer = listLobbiesTimerMax;
            ListLobbies();
            } 
        }
    }

    private void HandleHeartbeat()
    {
        if (IsLobbyHost())
        {
            heatbeatTimer += Time.deltaTime;
            if (heatbeatTimer <= 0)
            {
                float heartbeatTimerMax = 20;
                heatbeatTimer = heartbeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Filters = new List<QueryFilter>()
            {
                new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            }
            };

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            OnLobbyListChanged?.Invoke(this, new() { lobbyList = queryResponse.Results });
        }
        catch(LobbyServiceException e)
        {
            print(e);
        }
    }

    public async void CrateLobby(string lobbyName, bool isPrivate)
    {
        OnCreateLobbyStarted(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYERS_AMOUNT, new CreateLobbyOptions() { IsPrivate = isPrivate });

            KitchenGameMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.SceneName.CharacterSelectScene);
        }
        catch (LobbyServiceException e)
        {
            OnCreateLobbyFailed(this, EventArgs.Empty);
            print(e);
        }
    }

    public async void QuickJoin()
    {
        OnJoinStarted(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            OnQuickJoinFailed(this, EventArgs.Empty);
            print(e);
        }
    }

    public async void JoinWithCode(string code)
    {
        OnJoinStarted(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            OnJoinFailed(this, EventArgs.Empty);
            print(e);
        }
    }

    public async void JoinWithId(string id)
    {
        OnJoinStarted(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(id);
            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            OnJoinFailed(this, EventArgs.Empty);
            print(e);
        }
    }

    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                print(e);
            }
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                print(e);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                print(e);
            }
        }
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
