using KitchenChaos;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class DeliveryResultUI : MonoBehaviour
    {
        private const string POPUP = "Popup";
        private Animator animator;

        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI MessageText;
        [SerializeField] private Sprite successSprite;
        [SerializeField] private Sprite failedSprite;

        private readonly Color successColor = new(0f, 200f, 0f);
        private readonly Color failedColor = new(205f, 0f, 0f);

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerOnRecipeFailed;

            gameObject.SetActive(false);
        }

        private void DeliveryManagerOnRecipeSuccess(object sender, System.EventArgs e)
        {
            gameObject.SetActive(true);
            animator.SetTrigger(POPUP);
            backgroundImage.color = successColor;
            iconImage.sprite = successSprite;
            MessageText.text = "DELIVERY\nSUCCESS";
        }

        private void DeliveryManagerOnRecipeFailed(object sender, System.EventArgs e)
        {
            gameObject.SetActive(true);
            animator.SetTrigger(POPUP);
            backgroundImage.color = failedColor;
            iconImage.sprite = failedSprite;
            MessageText.text = "DELIVERY\nFAILED";
        }
    }
}