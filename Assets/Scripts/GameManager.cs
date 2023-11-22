using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenChaos
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event EventHandler OnGameStateChanged;
        public event EventHandler OnGamePaused;
        public event EventHandler OnGameUnpaused;
        public event EventHandler OnLocalPlayerReadyChanged;

        private enum GameState
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }

        private NetworkVariable<GameState> gameState = new(GameState.WaitingToStart);

        private bool isGamePaused = false;
        private bool isLocalPlayerReady;
        private NetworkVariable<float> countdownToStartTimer = new(3f);
        private NetworkVariable<float> gamePlayingTimer = new(0f);
        private readonly float gamePlayingTimerMax = 90f;

        private Dictionary<ulong, bool> playerReadyDictionary;

        public static bool IsFirstGame { get; set; } = true;
        public bool IsLocalPlayerReady { get => isLocalPlayerReady; }
        public float CountdownToStartTimer { get => countdownToStartTimer.Value; private set => countdownToStartTimer.Value = value; }
        public float GamePlayingTimer { get => gamePlayingTimer.Value; private set => gamePlayingTimer.Value = value; }

        private void Awake()
        {
            Instance = this;

            playerReadyDictionary = new Dictionary<ulong, bool>();
        }

        private void Start()
        {
            GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
            GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
        }

        public override void OnNetworkSpawn()
        {
            gameState.OnValueChanged += GameStateOnValueChanged;
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
            isGamePaused = !isGamePaused;
            if (isGamePaused)
            {
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            }

            Time.timeScale = isGamePaused ? 0f : 1f;
        }

        public bool IsGameWaitingToStart()
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
    }
}