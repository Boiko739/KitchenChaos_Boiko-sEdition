using KitchenChaos;
using System;
using TMPro;
using UnityEngine;

namespace MyUIs
{
    public class GameCountdownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownText;

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
            gameObject.SetActive(false);
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            gameObject.SetActive(GameManager.Instance.IsCountdownToStart());
        }

        private void Update()
        {
            countdownText.text = Math.Ceiling(GameManager.Instance.GetCountdownToStartTimer()).ToString();
        }
    }
}