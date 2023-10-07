using KitchenChaos;
using MySOs;
using System;
using UnityEngine;

namespace Counters
{
    public class ContainerCounter : BaseCounter
    {
        public event EventHandler OnPlayerGrabbedObject;

        [SerializeField] private KitchenObjectSO kitchenObjectSO;

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}