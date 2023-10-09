using Counters;
using UnityEngine;

namespace KitchenChaos
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
        }

        private void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            bool playSound = e.State is StoveCounter.State.Frying or StoveCounter.State.Fried;
            if (playSound)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Pause();
            }
        }
    }
}