using Counters;
using System;
using UnityEngine;

namespace OtherScripts
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
            stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
            stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            GameManager.Instance.OnLocalGamePaused += GameManager_OnLocalGamePaused;
            GameManager.Instance.OnLocalGameUnpaused += GameManager_OnLocalGameUnpaused;
        }

        private void GameManager_OnLocalGameUnpaused(object sender, EventArgs e)
        {
            if (PlaySound)
            {
                audioSource.Play();
            }
        }

        private void GameManager_OnLocalGamePaused(object sender, EventArgs e)
        {
            audioSource.Pause();
        }

        private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            playWarningSound = stoveCounter.CounterState.Value == StoveCounter.State.Fried
                            && e.ProgressNormalized >= e.minWarningProgressAmount;
        }

        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
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