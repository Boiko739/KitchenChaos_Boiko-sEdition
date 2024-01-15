using OtherScripts;

namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public static DeliveryCounter Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public override void Interact(Player player)
        {
            if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //only accept plates
                DeliveryManager.Instance.DeliveryRecipe(plateKitchenObject);
                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
            }
        }
    }
}