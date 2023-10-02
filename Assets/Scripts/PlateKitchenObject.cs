using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO ingredient;

        public OnIngredientAddedEventArgs(KitchenObjectSO kitchenObjectSO)
        {
            ingredient = kitchenObjectSO;
        }
    }

    [SerializeField]
    private List<KitchenObjectSO> invalidKitchenObjectSOList;
    [SerializeField] private List<KitchenObjectSO> invalidKitchenObjectSOList_ChaosMode;

    public List<KitchenObjectSO> kitchenObjectSOList { get; private set; }

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryToAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (kitchenObjectSOList.Contains(kitchenObjectSO) || invalidKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //The ingredient is invalid OR already has this ingredient
            return false;
        }
        else
        {
            //Doesn't have this ingredient yet AND
            //The ingredient is valid
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs(kitchenObjectSO));
            return true;
        }
    }
}
