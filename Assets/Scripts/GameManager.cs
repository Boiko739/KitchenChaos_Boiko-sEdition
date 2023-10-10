using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace KitchenChaos
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event EventHandler OnStateChanged;
        private enum GameState
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }

        private GameState gameState;

        private float waitingToStartTimer = 1f;
        private float countdownToStartTimer = 3f;
        private float gamePlayingTimer = 10f;

        private void Awake()
        {
            Instance = this;
            gameState = GameState.WaitingToStart;
        }

        void Update()
        {
            switch (gameState)
            {
                case GameState.WaitingToStart:
                    waitingToStartTimer -= Time.deltaTime;
                    if (waitingToStartTimer <= 0)
                    {
                        gameState = GameState.CountdownToStart;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }

                    break;
                case GameState.CountdownToStart:
                    countdownToStartTimer -= Time.deltaTime;
                    if (countdownToStartTimer <= 0)
                    {
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

        public bool IsGamePlaying()
        {
            return gameState == GameState.GamePlaying;
        }

        public bool IsCountdownToStart()
        {
            return gameState == GameState.CountdownToStart;
        }

        public float GetCountdownToStartTimer()
        {
            return countdownToStartTimer;
        }
    }
}