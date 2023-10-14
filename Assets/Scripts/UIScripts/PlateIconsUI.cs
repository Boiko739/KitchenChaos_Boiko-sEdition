using KitchenChaos;
using MySOs;
using UnityEngine;

namespace MyUIs
{
    public class PlateIconsUI : MonoBehaviour
    {
        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField] private Transform iconTemplate;

        private void Start()
        {
            iconTemplate.gameObject.SetActive(false);
            plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
        }

        private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            foreach (Transform child in transform)
            {
                if (child != iconTemplate)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < plateKitchenObject.KitchenObjectSOList.Count; i++)
            {
                KitchenObjectSO kitchenObjectSO = plateKitchenObject.KitchenObjectSOList[i];
                Transform iconInstance = Instantiate(iconTemplate, transform);
                iconInstance.gameObject.SetActive(true);
                iconInstance.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
            }
        }
    }
}