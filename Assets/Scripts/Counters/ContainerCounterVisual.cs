using UnityEngine;

namespace Counters
{
    public class ContainerCounterVisual : MonoBehaviour
    {
        private const string OPEN_CLOSE = "OpenClose";

        [SerializeField] private ContainerCounter containerCounter;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            containerCounter.OnPlayerGrabbedObject += ContainerCounterOnPlayerGrabbedObject;
        }

        private void ContainerCounterOnPlayerGrabbedObject(object sender, System.EventArgs e)
        {
            animator.SetTrigger(OPEN_CLOSE);
        }
    }
}