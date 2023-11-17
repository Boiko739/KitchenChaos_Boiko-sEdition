using KitchenChaos;
using UnityEngine;

namespace Counters
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] BaseCounter baseCounter;
        [SerializeField] GameObject[] visualGameObjectArray;
        private void Start()
        {
            if (Player.LocalInstance != null)
            {
                Player.LocalInstance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
            }
            else
            {
                Player.OnAnyPlayerSpawned += PlayerOnAnyPlayerSpawned;
            }
        }

        private void PlayerOnAnyPlayerSpawned(object sender, System.EventArgs e)
        {
            if (Player.LocalInstance != null)
            {
                Player.LocalInstance.OnSelectedCounterChanged -= PlayerOnSelectedCounterChanged;
                Player.LocalInstance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
            }
        }

        private void PlayerOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
        {
            if (e.SelectedCounter == baseCounter)
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