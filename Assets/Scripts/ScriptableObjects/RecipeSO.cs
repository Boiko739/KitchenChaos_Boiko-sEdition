using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    [SerializeField] public List<KitchenObjectSO> KitchenObjectSOList;
    [SerializeField] public string RecipeName;
}
