using KitchenChaos;
using MySOs;
using System;
using Unity.Netcode;
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
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractLogicPlaceObjectOnCounterServerRpc();
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
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
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
                                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
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

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicPlaceObjectOnCounterServerRpc()
        {
            InteractLogicPlaceObjectOnCounterClientRpc();
        }

        [ClientRpc]
        private void InteractLogicPlaceObjectOnCounterClientRpc()
        {
            cuttingProgress = 0;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                ProgressNormalized = 0f
            });
        }

        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
            {
                CutObjectServerRpc();
                TestCuttingProgressServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CutObjectServerRpc()
        {
            CutObjectClientRpc();
        }


        [ClientRpc]
        private void CutObjectClientRpc()
        {
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingReciteSO(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                ProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
        }

        [ServerRpc(RequireOwnership = false)]
        private void TestCuttingProgressServerRpc()
        {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingProgress >= GetCuttingReciteSO(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax)
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
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