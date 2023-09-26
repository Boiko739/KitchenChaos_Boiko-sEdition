using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler OnCut;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressBarChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    [SerializeField]
    public CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;

                CuttingRecipeSO cuttingRecipeSO = GetCuttingReciteSO(GetKitchenObject().GetKitchenObjectSO());
                OnProgressBarChanged?.Invoke(this, new OnProgressChangedEventArgs()
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
            }
        }
        else if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            OnCut?.Invoke(this, EventArgs.Empty);

            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingReciteSO(GetKitchenObject().GetKitchenObjectSO());
            OnProgressBarChanged?.Invoke(this, new OnProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingProgress >= GetCuttingReciteSO(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private CuttingRecipeSO GetCuttingReciteSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingReciteSO(inputKitchenObjectSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        var cuttingRecipe = GetCuttingReciteSO(inputKitchenObjectSO);
        return cuttingRecipe == null ? null : cuttingRecipe.output;
    }
}
