using KitchenChaos;
using TMPro;
using UnityEngine;

namespace MyUIs
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipesDeliveredText;

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
            gameObject.SetActive(false);
        }

        private void GameManagerOnStateChanged(object sender, System.EventArgs e)
        {
            if (GameManager.Instance.IsGameOver())
            {
                gameObject.SetActive(true);
                recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulDeliveriesAmount.ToString();
            }
        }
    }
}