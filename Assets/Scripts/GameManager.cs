using System;
using UnityEngine;

namespace KitchenChaos
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event EventHandler OnStateChanged;
        public event EventHandler OnGamePaused;
        public event EventHandler OnGameUnpaused;

        private enum GameState
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }

        private GameState gameState;

        private bool isGamePaused = false;
        private float countdownToStartTimer = 3f;
        private readonly float gamePlayingTimerMax = 30f;
        private float gamePlayingTimer;

        private void Awake()
        {
            Instance = this;

            gameState = GameState.WaitingToStart;
        }

        private void Start()
        {
            GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
            GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
        }

        private void GameInputOnInteractAction(object sender, EventArgs e)
        {
            if (gameState == GameState.WaitingToStart)
            {
                gameState = GameState.CountdownToStart;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GameInputOnPauseAction(object sender, EventArgs e)
        {
            TogglePauseGame();
        }

        void Update()
        {
            switch (gameState)
            {
                case GameState.WaitingToStart:
                    break;

                case GameState.CountdownToStart:
                    countdownToStartTimer -= Time.deltaTime;
                    if (countdownToStartTimer <= 0)
                    {
                        gamePlayingTimer = gamePlayingTimerMax;

                        gameState = GameState.GamePlaying;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }

                    break;

                case GameState.GamePlaying:
                    gamePlayingTimer -= Time.deltaTime;
                    if (gamePlayingTimer <= 0)
                    {
                        gameState = GameState.GameOver;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
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
            return gameState == GameState.WaitingToStart;
        }

        public bool IsGamePlaying()
        {
            return gameState == GameState.GamePlaying;
        }

        public bool IsCountdownToStart()
        {
            return gameState == GameState.CountdownToStart;
        }

        public bool IsGameOver()
        {
            return gameState == GameState.GameOver;
        }

        public float GetCountdownToStartTimer()
        {
            return countdownToStartTimer;
        }

        public float GetGamePlayingTimerNormalized()
        {
            return 1f - (gamePlayingTimer / gamePlayingTimerMax);
        }
    }
}