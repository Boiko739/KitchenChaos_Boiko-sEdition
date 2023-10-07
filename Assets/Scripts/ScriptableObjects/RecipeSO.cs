using System.Collections.Generic;
using UnityEngine;

namespace MySOs
{
    [CreateAssetMenu()]
    public class RecipeSO : ScriptableObject
    {
        public List<KitchenObjectSO> KitchenObjectSOList;
        public string RecipeName;
    }
}