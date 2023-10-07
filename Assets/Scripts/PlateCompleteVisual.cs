using MySOs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos
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
            plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
            foreach (KitchenObjectSOGameObject pair in kitchenObjectSOGameObjectsList)
            {
                pair.GameObject.SetActive(false);
            }
        }

        private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
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