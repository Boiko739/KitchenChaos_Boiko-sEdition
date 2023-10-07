using UnityEngine;

namespace MySOs
{
    [CreateAssetMenu()]
    public class CuttingRecipeSO : ScriptableObject
    {
        public KitchenObjectSO input;
        public KitchenObjectSO output;
        public int cuttingProgressMax;
    }
}