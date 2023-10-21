using Counters;
using UnityEngine;

namespace MyUIs
{
    public class StoveBurnWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private readonly float showWarningProgressAmount = .4f;

        private void Start()
        {
            stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
            gameObject.SetActive(false);
        }

        private void StoveCounterOnProgressChanged(object sender, KitchenChaos.IHasProgress.OnProgressChangedEventArgs e)
        {
            gameObject.SetActive(stoveCounter.CounterState == StoveCounter.State.Fried && e.ProgressNormalized >= showWarningProgressAmount);
        }
    }
}