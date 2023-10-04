using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> KitchenObjectSOList { get; set; }
    public string RecipeName { get; set; }
}
