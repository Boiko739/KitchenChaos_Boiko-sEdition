using Counters;
using UnityEngine;

namespace MyUIs
{
    public class StoveProgressBarWarningUI : MonoBehaviour
    {
        private const string IS_FLASHING = "IsFlashing";

        [SerializeField] private StoveCounter stoveCounter;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
            animator.SetBool(IS_FLASHING, false);
        }

        private void StoveCounterOnProgressChanged(object sender, KitchenChaos.IHasProgress.OnProgressChangedEventArgs e)
        {
            animator.SetBool(IS_FLASHING, e.ProgressNormalized >= e.minWarningProgressAmount
                                          && stoveCounter.CounterState == StoveCounter.State.Fried);
        }
    }
}