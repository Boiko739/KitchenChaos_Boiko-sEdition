using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            if (waitingRecipeSOList[i] != null && plateKitchenObject.KitchenObjectSOList.Count == waitingRecipeSOList[i].KitchenObjectSOList.Count)
            {
                //Has the same amount of ingredients
                if (true)
                {//plateKitchenObject.Equals(waitingRecipeSOList[i])
                    print("0k");
                } 
            }
        }
    }
}