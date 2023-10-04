using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public OnIngredientAddedEventArgs(KitchenObjectSO kitchenObjectSO)
        {
            Ingredient = kitchenObjectSO;
        }

        public KitchenObjectSO Ingredient { get; set; }
    }

    [SerializeField] private List<KitchenObjectSO> invalidKitchenObjectSOList;
    [SerializeField] private List<KitchenObjectSO> invalidKitchenObjectSOList_ChaosMode;

    public List<KitchenObjectSO> KitchenObjectSOList { get; private set; }

    private void Awake()
    {
        KitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryToAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (KitchenObjectSOList.Contains(kitchenObjectSO) || invalidKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //The ingredient is invalid OR it already has this ingredient
            return false;
        }
        else
        {
            //Doesn't have this ingredient yet AND
            //The ingredient is valid
            KitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs(kitchenObjectSO));
            return true;
        }
    }

    public bool Equals(RecipeListSO recipeListSO)
    {
        foreach (KitchenObjectSO kitchenObjectSO in KitchenObjectSOList)
        {
            bool ingredientFound = false;
            foreach (RecipeSO ingredientSO in recipeListSO.RecipeSOList)
            {
                if (ingredientSO == kitchenObjectSO)
                {
                    ingredientFound = true;
                    break;
                }
            }

            if (!ingredientFound)
            {
                return false;
            }
        }

        return true;
    }
}
