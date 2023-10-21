using KitchenChaos;
using MySOs;
using System;
using UnityEngine;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public static event EventHandler OnAnyCut;

        new public static void ResetStaticData()
        {
            OnAnyCut = null;
        }

        public event EventHandler OnCut;
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

        [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

        private int cuttingProgress;

        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                //The counter is empty
                if (player.HasKitchenObject())
                {
                    //The player is holding some KitchenObject
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingReciteSO(GetKitchenObject().GetKitchenObjectSO());
                    if (cuttingRecipeSO != null)
                    {
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                        {
                            ProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                        });
                    }
                }
            }
            else
            {
                //There is something on the counter
                if (!player.HasKitchenObject())
                {
                    //The player isn't holding any KitchenObject
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
                else
                {
                    //The player is holding some KitchenObject
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                    {
                        //The player is holding a plate
                        if (plateKitchenObject.TryToAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            //The player is adding an ingredient
                            GetKitchenObject().DestroySelf();
                        }
                    }
                    else
                    {
                        //The player honding some object but not a plate
                        if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                        {
                            //Therer is a plate on the counter
                            if (plateKitchenObject.TryToAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                            {
                                player.GetKitchenObject().DestroySelf();
                            }
                        }
                        else
                        {
                            //The ingredient is invalid
                        }
                    }
                }
            }
        }

        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
            {
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);

                cuttingProgress++;
                CuttingRecipeSO cuttingRecipeSO = GetCuttingReciteSO(GetKitchenObject().GetKitchenObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    ProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
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
}