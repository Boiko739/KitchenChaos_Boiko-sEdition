using KitchenChaos;
using System;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlacedHere;

        public static void ResetStaticData()
        {
            OnAnyObjectPlacedHere = null;
        }

        [SerializeField] private Transform counterTopPoint;

        private KitchenObject kitchenObject;

        public virtual void Interact(Player player)
        {
            Debug.LogError("BaseCounter.Interact(Player player);");
        }

        public virtual void InteractAlternate(Player player)
        { }

        public Transform GetKitchenObjectFollowTransform()
        {
            return counterTopPoint;
        }

        public KitchenObject GetKitchenObject()
        {
            return kitchenObject;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;

            if (kitchenObject != null)
            {
                OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ClearKitchenObject()
        {
            kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return kitchenObject != null;
        }
    }
}