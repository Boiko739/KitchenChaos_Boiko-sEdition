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
            stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            gameObject.SetActive(false);
        }

        private void StoveCounter_OnProgressChanged(object sender, KitchenChaos.IHasProgress.OnProgressChangedEventArgs e)
        {
            gameObject.SetActive(stoveCounter.CounterState.Value == StoveCounter.State.Fried && e.ProgressNormalized >= showWarningProgressAmount);
        }
    }
}