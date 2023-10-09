using KitchenChaos;
using MySOs;
using UnityEngine;

namespace MyUIs
{
    public class DeliveryManagerUI : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private Transform recipeTemplate;

        private void Awake()
        {
            recipeTemplate.gameObject.SetActive(false);
        }

        private void Start()
        {
            DeliveryManager.Instance.OnRecipeSpawned += DeliveryManagerInstanceOnRecipeSpawned;
            DeliveryManager.Instance.OnRecipeCompleted += DeliveryManagerInstanceOnRecipeCompleted;
            UpdateVisual();
        }

        private void DeliveryManagerInstanceOnRecipeSpawned(object sender, System.EventArgs e)
        {
            UpdateVisual();
        }

        private void DeliveryManagerInstanceOnRecipeCompleted(object sender, System.EventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            foreach (Transform child in container)
            {
                if (child != recipeTemplate)
                {
                    Destroy(child.gameObject);
                }
            }

            var waitingRecipes = DeliveryManager.Instance.WaitingRecipeSOList;
            for (int i = 0; i < waitingRecipes.Count; i++)
            {
                RecipeSO recipeSO = waitingRecipes[i];
                Transform recipeTransform = Instantiate(recipeTemplate, container);
                recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
                recipeTransform.gameObject.SetActive(true);
            }
        }
    }
}