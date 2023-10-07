using UnityEngine;

namespace Counters
{
    public class StoveCounterVisual : MonoBehaviour
    {
        [SerializeField] StoveCounter baseCounter;
        [SerializeField] GameObject[] visualGameObjectArray;

        private void Start()
        {
            baseCounter.OnStateChanged += BaseCounterOnStateChanged;
        }

        private void BaseCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            if (e.State is StoveCounter.State.Frying or StoveCounter.State.Fried)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            foreach (var visualGameObject in visualGameObjectArray)
            {
                visualGameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            foreach (var visualGameObject in visualGameObjectArray)
            {
                visualGameObject.SetActive(false);
            }
        }
    }
}