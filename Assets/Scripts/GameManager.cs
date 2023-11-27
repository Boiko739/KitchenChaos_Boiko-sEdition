using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Properties;
using UnityEngine;

namespace KitchenChaos
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event EventHandler OnGameStateChanged;
        public event EventHandler OnLocalGamePaused;
        public event EventHandler OnLocalGameUnpaused;
        public event EventHandler OnMultiplayerGamePaused;
        public event EventHandler OnMultiplayerGameUnpaused;
        public event EventHandler OnLocalPlayerReadyChanged;

        private enum GameState
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }

        private readonly NetworkVariable<GameState> gameState = new(GameState.WaitingToStart);
        private readonly NetworkVariable<bool> isGamePaused = new(false);

        private bool isLocalGamePaused = false;
        private bool isLocalPlayerReady;
        private bool autoTestGamePausedState;

        private readonly NetworkVariable<float> countdownToStartTimer = new(3f);
        private readonly NetworkVariable<float> gamePlayingTimer = new(0f);

        private readonly float gamePlayingTimerMax = 90f;

        private Dictionary<ulong, bool> playerReadyDictionary;
        private Dictionary<ulong, bool> playerPausedDictionary;

        public static bool IsFirstGame { get; set; } = true;
        public bool IsLocalPlayerReady { get => isLocalPlayerReady; }
        public float CountdownToStartTimer { get => countdownToStartTimer.Value; private set => countdownToStartTimer.Value = value; }
        public float GamePlayingTimer { get => gamePlayingTimer.Value; private set => gamePlayingTimer.Value = value; }

        private void Awake()
        {
            Instance = this;

            playerReadyDictionary = new Dictionary<ulong, bool>();
            playerPausedDictionary = new Dictionary<ulong, bool>();
        }

        public void StartHost()
        {
            NetworkManager.ConnectionApprovalCallback += NetworkManagerConnectionApprovalCallback;
            NetworkManager.StartHost();
        }

        private void NetworkManagerConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {
            connectionApprovalResponse.Approved = Instance.IsWaitingToStart();
            connectionApprovalResponse.CreatePlayerObject = Instance.IsWaitingToStart();
        }

        public void StartClient()
        {
            NetworkManager.StartClient();
        }

        private void Start()
        {
            GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
            GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
        }

        public override void OnNetworkSpawn()
        {
            gameState.OnValueChanged += GameStateOnValueChanged;
            isGamePaused.OnValueChanged += IsGamePausedOnValueChanged;

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            }
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            autoTestGamePausedState = true;
        }

        private void IsGamePausedOnValueChanged(bool previousValue, bool newValue)
        {
            if (isGamePaused.Value)
            {
                OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0f;
            }
            else
            {
                OnMultiplayerGameUnpaused?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 1f;
            }
        }

        private void GameStateOnValueChanged(GameState previousValue, GameState newValue)
        {
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void GameInputOnInteractAction(object sender, EventArgs e)
        {
            if (gameState.Value == GameState.WaitingToStart)
            {
                isLocalPlayerReady = true;
                OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

                SetPlayerReadyServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
                {
                    return;
                }
            }

            //All clients are ready
            gameState.Value = GameState.CountdownToStart;
        }

        private void GameInputOnPauseAction(object sender, EventArgs e)
        {
            TogglePauseGame();
        }

        private void LateUpdate()
        {
            if (autoTestGamePausedState)
            {
                autoTestGamePausedState = false;
                TestGamePausedState();
            }
        }

        void Update()
        {
            if (!IsServer)
            {
                return;
            }

            switch (gameState.Value)
            {
                case GameState.WaitingToStart:
                    if (!IsFirstGame)
                    {
                        GameInputOnInteractAction(null, EventArgs.Empty);
                    }

                    break;

                case GameState.CountdownToStart:
                    CountdownToStartTimer -= Time.deltaTime;
                    if (CountdownToStartTimer <= 0)
                    {
                        GamePlayingTimer = gamePlayingTimerMax;

                        gameState.Value = GameState.GamePlaying;
                        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                    }

                    break;

                case GameState.GamePlaying:
                    GamePlayingTimer -= Time.deltaTime;
                    if (GamePlayingTimer <= 0)
                    {
                        gameState.Value = GameState.GameOver;
                    }

                    break;

                case GameState.GameOver:
                    break;
            }
        }

        public void TogglePauseGame()
        {
            isLocalGamePaused = !isLocalGamePaused;
            if (isLocalGamePaused)
            {
                PauseGameServerRpc();
                OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                UnpauseGameServerRpc();
                OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsWaitingToStart()
        {
            return gameState.Value == GameState.WaitingToStart;
        }

        public bool IsGamePlaying()
        {
            return gameState.Value == GameState.GamePlaying;
        }

        public bool IsCountdownToStart()
        {
            return gameState.Value == GameState.CountdownToStart;
        }

        public bool IsGameOver()
        {
            return gameState.Value == GameState.GameOver;
        }

        public float GetGamePlayingTimerNormalized()
        {
            return 1f - (GamePlayingTimer / gamePlayingTimerMax);
        }

        [ServerRpc(RequireOwnership = false)]
        private void PauseGameServerRpc(ServerRpcParams rpcParams = default)
        {
            playerPausedDictionary[rpcParams.Receive.SenderClientId] = true;
            TestGamePausedState();
        }

        [ServerRpc(RequireOwnership = false)]
        private void UnpauseGameServerRpc(ServerRpcParams rpcParams = default)
        {
            playerPausedDictionary[rpcParams.Receive.SenderClientId] = false;
            TestGamePausedState();
        }

        private void TestGamePausedState()
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
                {
                    //This player is paused
                    isGamePaused.Value = true;
                    return;
                }

                //All players are unpaused
                isGamePaused.Value = false;
            }
        }
    }
}