using KitchenChaos;
using MySOs;
using UnityEngine;

namespace Counters
{
    public class ClearCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;

        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                //The counter is empty
                if (player.HasKitchenObject())
                {
                    //The player is holding some KitchenObject
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
            else
            {
                //There is some KitchenObject on the counter
                if (!player.HasKitchenObject())
                {
                    //The player isn't holding any KitchenObject
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
                else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //The player is holding a Plate
                    if (plateKitchenObject.TryToAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //Player is holding some object but not a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //There is a plate on the counter
                        if (plateKitchenObject.TryToAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}