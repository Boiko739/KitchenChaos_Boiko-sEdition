using OtherScripts;
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
                Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
            }
            else
            {
                Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
            }
        }

        private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
        {
            if (Player.LocalInstance != null)
            {
                Player.LocalInstance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
                Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
            }
        }

        private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
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