using Unity.VisualScripting;
using UnityEngine;

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
            if (child == iconTemplate)
            { 
                //This transform is a template
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.kitchenObjectSOList)
        {
            Transform iconInstance = Instantiate(iconTemplate, transform);
            iconInstance.gameObject.SetActive(true);
            iconInstance.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
