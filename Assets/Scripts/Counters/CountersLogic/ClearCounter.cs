using OtherScripts;
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
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    //Player is holding some object but not a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject) &&
                        plateKitchenObject.TryToAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        //There is a plate on the counter and the ingredient can be added
                        KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                    }
                }
            }
        }
    }
}