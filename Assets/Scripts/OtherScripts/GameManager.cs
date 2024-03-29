using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace OtherScripts
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

        [SerializeField] private Transform playerPrefab;

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

        private void Start()
        {
            GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
            GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        }

        public override void OnNetworkSpawn()
        {
            gameState.OnValueChanged += GameState_OnValueChanged;
            isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            }
        }

        private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Transform playerTransform = Instantiate(playerPrefab);
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            }
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            autoTestGamePausedState = true;
        }

        private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
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

        private void GameState_OnValueChanged(GameState previousValue, GameState newValue)
        {
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void GameInput_OnInteractAction(object sender, EventArgs e)
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

        private void GameInput_OnPauseAction(object sender, EventArgs e)
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
                        GameInput_OnInteractAction(null, EventArgs.Empty);
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