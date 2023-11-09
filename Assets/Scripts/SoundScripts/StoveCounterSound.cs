using Counters;
using System;
using UnityEngine;

namespace KitchenChaos
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private AudioSource audioSource;
        private float warningSoundTimer = .2f;
        private readonly float warningSoundTimerMax = .2f;
        private bool playWarningSound;
        public bool PlaySound { get; private set; }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
            stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
            GameManager.Instance.OnGamePaused += GameManagerOnGamePaused;
            GameManager.Instance.OnGameUnpaused += GameManagerOnGameunpaused;
        }

        private void GameManagerOnGameunpaused(object sender, EventArgs e)
        {
            if (PlaySound)
            {
                audioSource.Play();
            }
        }

        private void GameManagerOnGamePaused(object sender, EventArgs e)
        {
            audioSource.Pause();
        }

        private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            playWarningSound = stoveCounter.CounterState == StoveCounter.State.Fried
                            && e.ProgressNormalized >= e.minWarningProgressAmount;
        }

        private void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            PlaySound = e.State is StoveCounter.State.Frying or StoveCounter.State.Fried;
            if (PlaySound)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Pause();
            }
        }

        private void Update()
        {
            if (playWarningSound)
            {
                warningSoundTimer -= Time.deltaTime;
                if (warningSoundTimer <= 0)
                {
                    warningSoundTimer = warningSoundTimerMax;
                    SoundManager.Instance.PlayBurnWarningSound(stoveCounter);
                }
            }
        }
    }
}