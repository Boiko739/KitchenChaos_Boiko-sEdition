public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
    }

    public override void InteractAlternate(Player player)
    {
        //Do nothing
    }
}
