using KitchenChaos;
using System;
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
                OnAnyTrashThrowen?.Invoke(this, EventArgs.Empty);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}