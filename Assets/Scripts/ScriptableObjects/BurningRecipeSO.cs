using UnityEngine;

namespace MySOs
{
    [CreateAssetMenu()]
    public class BurningRecipeSO : ScriptableObject
    {
        public KitchenObjectSO input;
        public KitchenObjectSO output;
        public float burningTimerMax;
    }
}