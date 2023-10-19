using MySOs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos
{
    public class DeliveryManager : MonoBehaviour
    {
        public static DeliveryManager Instance { get; private set; }

        public event EventHandler OnRecipeSpawned;
        public event EventHandler OnRecipeCompleted;

        public event EventHandler OnRecipeSuccess;
        public event EventHandler OnRecipeFailed;

        [SerializeField] private RecipeListSO recipeListSO;

        public List<RecipeSO> WaitingRecipeSOList { get; private set; }

        public int SuccessfulDeliveriesAmount { get; private set; }
        private float spawnRecipeTimer;
        private readonly float spawnRecipeTimerMax = 4f;
        private readonly short waitingRecipesMax = 4;

        private void Awake()
        {
            Instance = this;
            WaitingRecipeSOList = new List<RecipeSO>();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGamePlaying() && spawnRecipeTimer >= spawnRecipeTimerMax)
            {
                spawnRecipeTimer = 0;
                if (WaitingRecipeSOList.Count < waitingRecipesMax)
                {
                    RecipeSO recipe = recipeListSO.RecipeSOList[UnityEngine.Random.Range(0, recipeListSO.RecipeSOList.Count)];
                    WaitingRecipeSOList.Add(recipe);
                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                }
            }

            spawnRecipeTimer += Time.deltaTime;
        }

        public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
        {
            bool deliveryMatched = false;
            for (int i = 0; i < WaitingRecipeSOList.Count; i++)
            {
                bool hasTheSameAmountOfIngredients = plateKitchenObject.KitchenObjectSOList.Count == WaitingRecipeSOList[i].KitchenObjectSOList.Count;
                if (hasTheSameAmountOfIngredients && DeliveryMatchesAnyOrder(plateKitchenObject))
                {
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    SuccessfulDeliveriesAmount++;
                    WaitingRecipeSOList.RemoveAt(i);
                    deliveryMatched = true;
                    break;
                }
            }

            if (!deliveryMatched)
            {
                OnRecipeFailed?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool DeliveryMatchesAnyOrder(PlateKitchenObject plateKitchenObject)
        {
            for (int i = 0; i < WaitingRecipeSOList.Count; i++)
            {
                if (plateKitchenObject.KitchenObjectSOList.Count != WaitingRecipeSOList[i].KitchenObjectSOList.Count)
                {
                    continue;
                }

                RecipeSO recipe = WaitingRecipeSOList[i];
                bool recipeMatches = true;
                for (int j = 0; j < recipe.KitchenObjectSOList.Count; j++)
                {
                    KitchenObjectSO ingredient = recipe.KitchenObjectSOList[j];
                    if (!plateKitchenObject.KitchenObjectSOList.Contains(ingredient))
                    {
                        recipeMatches = false;
                        break;
                    }
                }

                if (recipeMatches)
                {
                    return recipeMatches;
                }
            }

            return false;
        }
    }
}