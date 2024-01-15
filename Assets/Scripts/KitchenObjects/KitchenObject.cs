using MySOs;
using OtherScripts;
using Unity.Netcode;
using UnityEngine;

namespace OtherScripts
{
    public class KitchenObject : NetworkBehaviour
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;

        private IKitchenObjectParent thisKitchenObjectParent;
        private FollowTransform followTransform;

        protected virtual void Awake()
        {
            followTransform = GetComponent<FollowTransform>();
        }

        public KitchenObjectSO GetKitchenObjectSO()
        {
            return kitchenObjectSO;
        }

        public IKitchenObjectParent GetKitchenObjectParent()
        {
            return thisKitchenObjectParent;
        }

        public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
        {
            SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
        {
            SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
        }

        [ClientRpc]
        public void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
        {
            kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
            IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

            thisKitchenObjectParent?.ClearKitchenObject();

            thisKitchenObjectParent = kitchenObjectParent;

            if (kitchenObjectParent.HasKitchenObject())
            {
                Debug.LogError($"{kitchenObjectParent} already has a KitchenObject!");
            }

            kitchenObjectParent.SetKitchenObject(this);

            followTransform.TargetTransform = kitchenObjectParent.GetKitchenObjectFollowTransform();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void ClearKitchenObjectOnParent()
        {
            thisKitchenObjectParent.ClearKitchenObject();
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

        public static void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
        }
    }
}