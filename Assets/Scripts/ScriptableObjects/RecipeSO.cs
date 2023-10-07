using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<KitchenObjectSO> kitchenObjectSOList;
    [SerializeField] private string recipeName;

    public string RecipeName { get => recipeName; }
    public List<KitchenObjectSO> KitchenObjectSOList { get => kitchenObjectSOList; }
}