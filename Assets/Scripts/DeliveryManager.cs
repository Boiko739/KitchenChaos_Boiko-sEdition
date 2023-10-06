using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private readonly float spawnRecipeTimerMax = 4f;
    private readonly short waitingRecipesMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (spawnRecipeTimer >= spawnRecipeTimerMax)
        {
            spawnRecipeTimer = 0;
            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO recipe = recipeListSO.RecipeSOList[Random.Range(0, recipeListSO.RecipeSOList.Count)];
                waitingRecipeSOList.Add(recipe);
                print($"{recipe.RecipeName}!");
            }
        }

        spawnRecipeTimer += Time.deltaTime;
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        bool deliveryMatched = false;
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            bool hasTheSameAmountOfIngredients = plateKitchenObject.KitchenObjectSOList.Count == waitingRecipeSOList[i].KitchenObjectSOList.Count;
            if (hasTheSameAmountOfIngredients && DeliveryMatchesAnyOrder(plateKitchenObject))
            {
                print($"{waitingRecipeSOList[i].RecipeName} is cooked right! Well done!");
                waitingRecipeSOList.RemoveAt(i);
                deliveryMatched = true;
                break;
            }
        }

        if (!deliveryMatched)
        {
            print("Invalid recipe!");
        }
    }

    private bool DeliveryMatchesAnyOrder(PlateKitchenObject plateKitchenObject)
    {
        foreach (var recipe in waitingRecipeSOList)
        {
            bool recipeMatches = true;
            foreach (var ingredient in recipe.KitchenObjectSOList)
            {
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