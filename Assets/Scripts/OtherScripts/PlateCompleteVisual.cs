using MySOs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OtherScripts
{
    public class PlateCompleteVisual : MonoBehaviour
    {
        [Serializable]
        public struct KitchenObjectSOGameObject
        {
            public KitchenObjectSO KitchenObjectSO;
            public GameObject GameObject;
        }

        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField] private List<KitchenObjectSOGameObject> kitchenObjectSOGameObjectsList;

        private void Start()
        {
            plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
            foreach (KitchenObjectSOGameObject pair in kitchenObjectSOGameObjectsList)
            {
                pair.GameObject.SetActive(false);
            }
        }

        private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
        {
            foreach (KitchenObjectSOGameObject pair in kitchenObjectSOGameObjectsList)
            {
                if (pair.KitchenObjectSO == e.Ingredient)
                {
                    pair.GameObject.SetActive(true);
                }
            }
        }
    }
}