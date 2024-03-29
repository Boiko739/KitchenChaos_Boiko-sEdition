using OtherScripts;
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
            DeliveryManager.Instance.OnRecipeSpawned += DeliveryManagerInstance_OnRecipeSpawned;
            DeliveryManager.Instance.OnRecipeCompleted += DeliveryManagerInstance_OnRecipeCompleted;
            UpdateVisual();
        }

        private void DeliveryManagerInstance_OnRecipeSpawned(object sender, System.EventArgs e)
        {
            UpdateVisual();
        }

        private void DeliveryManagerInstance_OnRecipeCompleted(object sender, System.EventArgs e)
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