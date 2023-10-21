using KitchenChaos;
using System;
using TMPro;
using UnityEngine;

namespace MyUIs
{
    public class CountdownTextUI : MonoBehaviour
    {
        private const string NUMBER_POPUP = "NumberPopup";

        public static CountdownTextUI Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI countdownText;

        public event EventHandler OnCountdownNumberChanged;

        private Animator animator;
        private int previousCountdownNumber;

        private void Awake()
        {
            Instance = this;
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            int countdownNumber = (int)Math.Ceiling(GameManager.Instance.CountdownToStartTimer);
            countdownText.text = countdownNumber.ToString();

            if (countdownNumber != previousCountdownNumber)
            {
                previousCountdownNumber = countdownNumber;
                animator.SetTrigger(NUMBER_POPUP);
                OnCountdownNumberChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GameManagerOnStateChanged(object sender, EventArgs e)
        {
            gameObject.SetActive(GameManager.Instance.IsCountdownToStart());
        }
    }
}