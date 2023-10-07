using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    [SerializeField] private List<RecipeSO> recipeSOList;

    public List<RecipeSO> RecipeSOList { get => recipeSOList; }
}
