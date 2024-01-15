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
            stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            animator.SetBool(IS_FLASHING, false);
        }

        private void StoveCounter_OnProgressChanged(object sender, OtherScripts.IHasProgress.OnProgressChangedEventArgs e)
        {
            animator.SetBool(IS_FLASHING, e.ProgressNormalized >= e.minWarningProgressAmount
                                         && stoveCounter.CounterState.Value == StoveCounter.State.Fried);
        }
    }
}