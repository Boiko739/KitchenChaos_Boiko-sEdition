using KitchenChaos;
using MySOs;
using System;
using UnityEngine;

namespace Counters
{
    public class StoveCounter : BaseCounter, IHasProgress
    {
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

        public class OnStateChangedEventArgs : EventArgs
        {
            private State state;

            public OnStateChangedEventArgs(State state)
            {
                State = state;
            }

            public State State { get => state; private set => state = value; }
        }

        public enum State
        {
            Idle,
            Frying,
            Fried,
            Burned
        }

        [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
        [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

        private float fryingTimer;
        private float burningTimer;
        private FryingRecipeSO _fryingRecipeSO;
        private BurningRecipeSO _burningRecipeSO;
        private State state = State.Idle;

        private void Start()
        {
            state = State.Idle;
        }

        private void Update()
        {
            if (HasKitchenObject())
            {
                switch (state)
                {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer += Time.deltaTime;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = fryingTimer / _fryingRecipeSO.fryingTimerMax
                        });
                        if (fryingTimer >= _fryingRecipeSO.fryingTimerMax)
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                            state = State.Fried;
                            burningTimer = 0f;
                            _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
                        }

                        break;
                    case State.Fried:
                        burningTimer += Time.deltaTime;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = burningTimer / _burningRecipeSO.burningTimerMax
                        });
                        if (burningTimer >= _burningRecipeSO.burningTimerMax)
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);
                            state = State.Burned;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
                        }

                        break;
                    case State.Burned:
                        break;
                }
            }
        }

        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    KitchenObjectSO inputKitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                    if (GetFryingReciteSOWithInput(inputKitchenObjectSO) != null)
                    {
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        _fryingRecipeSO = GetFryingReciteSOWithInput(inputKitchenObjectSO);
                        state = State.Frying;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
                        fryingTimer = 0f;
                    }
                }
            }
            else
            {
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                    ResetTheStoveCounter();
                }
                else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)
                        && plateKitchenObject.TryToAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                {
                    //The player is holding a Plate
                    GetKitchenObject().DestroySelf();

                    ResetTheStoveCounter();
                }
            }
        }

        private void ResetTheStoveCounter()
        {
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(state));
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
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

        private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
            {
                if (burningRecipeSO.input == inputKitchenObjectSO)
                {
                    return burningRecipeSO;
                }
            }

            return null;
        }
    }
}