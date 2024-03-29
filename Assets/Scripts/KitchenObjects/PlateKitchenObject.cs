using MySOs;
using OtherScripts;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace OtherScripts
{
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

        protected override void Awake()
        {
            base.Awake();
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
                AddIngredientServerRpc(
                    KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO)
                );

                return true;
            }
        }
        [ServerRpc(RequireOwnership = false)]
        private void AddIngredientServerRpc(int kitchenObjectSOIndex)
        {
            AddIngredientClientRpc(kitchenObjectSOIndex);
        }

        [ClientRpc]
        private void AddIngredientClientRpc(int kitchenObjectSOIndex)
        {
            KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
            KitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs(kitchenObjectSO));
        }
    }
}