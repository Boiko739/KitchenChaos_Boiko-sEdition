using KitchenChaos;
using System;
using Unity.Netcode;
using UnityEngine.EventSystems;

namespace Counters
{
    public class TrashCounter : BaseCounter
    {
        public static event EventHandler OnAnyTrashThrowen;

        new public static void ResetStaticData()
        {
            OnAnyTrashThrowen = null;
        }

        public override void Interact(Player player)
        {
            if (player.HasKitchenObject())
            {
                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                InteractLogicServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void InteractLogicServerRpc()
        {
            InteractLogicClientRpc();
        }

        [ClientRpc]
        private void InteractLogicClientRpc()
        {
            OnAnyTrashThrowen?.Invoke(this, EventArgs.Empty);
        }
    }
}