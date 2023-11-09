using MySOs;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos
{
    public class KitchenObject : NetworkBehaviour
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;

        private IKitchenObjectParent kitchenObjectParent;

        public KitchenObjectSO GetKitchenObjectSO()
        {
            return kitchenObjectSO;
        }

        public IKitchenObjectParent GetKitchenObjectParent()
        {
            return kitchenObjectParent;
        }

        public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
        {
            this.kitchenObjectParent?.ClearKitchenObject();

            this.kitchenObjectParent = kitchenObjectParent;

            if (kitchenObjectParent.HasKitchenObject())
            {
                Debug.LogError($"{kitchenObjectParent} already has a KitchenObject!");
            }

            kitchenObjectParent.SetKitchenObject(this);

            //transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
            //transform.localPosition = Vector3.zero;
        }

        public void DestroySelf()
        {
            kitchenObjectParent.ClearKitchenObject();
            Destroy(gameObject);
        }

        public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
        {
            if (GetType() == typeof(PlateKitchenObject))
            {
                plateKitchenObject = this as PlateKitchenObject;
                return true;
            }
            else
            {
                plateKitchenObject = null;
                return false;
            }
        }

        public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
        {
            KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
        }
    }
}