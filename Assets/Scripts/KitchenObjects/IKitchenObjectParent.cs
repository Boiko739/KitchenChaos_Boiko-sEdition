using Unity.Netcode;
using UnityEngine;

namespace OtherScripts
{
    public interface IKitchenObjectParent
    {
        public Transform GetKitchenObjectFollowTransform();

        public KitchenObject GetKitchenObject();

        public void SetKitchenObject(KitchenObject kitchenObject);

        public void ClearKitchenObject();

        public bool HasKitchenObject();

        public NetworkObject GetNetworkObject();
    }
}