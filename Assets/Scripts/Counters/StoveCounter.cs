using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    private float fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private State state = State.Idle;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    if (fryingTimer >= _fryingRecipeSO.fryingTimerMax)
                    {
                        print("Fried!");
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        _fryingRecipeSO = GetFryingReciteSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Fried;
                    }
                    break;
                case State.Fried:
                    break;
                case State.Burned:
                    break;
            }
            print(state);
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                KitchenObjectSO inputKitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                if (HasRecipeWithInput(inputKitchenObjectSO))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _fryingRecipeSO = GetFryingReciteSOWithInput(inputKitchenObjectSO);
                    state = State.Frying;
                    fryingTimer = 0f;
                }
            }
        }
        else if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }

    public override void InteractAlternate(Player player)
    {

    }
    private FryingRecipeSO GetFryingReciteSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingReciteSOWithInput(inputKitchenObjectSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        var fryingRecipe = GetFryingReciteSOWithInput(inputKitchenObjectSO);
        return fryingRecipe == null ? null : fryingRecipe.output;
    }
}
