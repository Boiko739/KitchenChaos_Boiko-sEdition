using MySOs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyUIs
{
    public class DeliveryManagerSingleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeNameText;
        [SerializeField] private Transform iconContainer;
        [SerializeField] private Transform iconTemplate;

        private void Awake()
        {
            iconTemplate.gameObject.SetActive(false);
        }

        public void SetRecipeSO(RecipeSO recipeSO)
        {
            recipeNameText.text = recipeSO.RecipeName;

            foreach (Transform child in iconContainer)
            {
                if (child != iconTemplate)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < recipeSO.KitchenObjectSOList.Count; i++)
            {
                KitchenObjectSO kitchenObjectSO = recipeSO.KitchenObjectSOList[i];
                Transform iconTransform = Instantiate(iconTemplate, iconContainer);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
            }
        }
    }
}