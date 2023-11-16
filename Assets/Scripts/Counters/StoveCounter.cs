using KitchenChaos;
using MySOs;
using System;
using Unity.Netcode;
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

        private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
        private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);

        private FryingRecipeSO fryingRecipeSO;
        private BurningRecipeSO burningRecipeSO;

        public NetworkVariable<State> CounterState { get; private set; } = new NetworkVariable<State>(State.Idle);

        public override void OnNetworkSpawn()
        {
            fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
            burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
            CounterState.OnValueChanged += CounterState_OnValueChanged;
        }

        private void CounterState_OnValueChanged(State previousState, State newState)
        {
            OnStateChanged.Invoke(this, new OnStateChangedEventArgs(CounterState.Value));

            if (CounterState.Value is State.Idle or State.Burned)
            {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
            }
        }

        private void BurningTimer_OnValueChanged(float previousValue, float newValue)
        {
            float burningTimerMax = burningTimer != null ? burningRecipeSO.burningTimerMax : 1f;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                ProgressNormalized = burningTimer.Value / burningTimerMax
            });
        }

        private void FryingTimer_OnValueChanged(float previousValue, float newValue)
        {
            float fryingTimerMax = fryingTimer != null ? fryingRecipeSO.fryingTimerMax : 1f;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                ProgressNormalized = fryingTimer.Value / fryingTimerMax
            });
        }

        private void Update()
        {
            if (!IsServer)
            {
                return;
            }

            if (HasKitchenObject())
            {
                switch (CounterState.Value)
                {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer.Value += Time.deltaTime;

                        if (fryingTimer.Value >= fryingRecipeSO.fryingTimerMax)
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());

                            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                            CounterState.Value = State.Fried;
                            burningTimer.Value = 0f;

                            SetBurningRecipeSOClientRpc(
                                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO())
                             );
                        }

                        break;
                    case State.Fried:
                        burningTimer.Value += Time.deltaTime;

                        if (burningTimer.Value >= burningRecipeSO.burningTimerMax)
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                            CounterState.Value = State.Burned;
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
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    KitchenObjectSO inputKitchenObjectSO = kitchenObject.GetKitchenObjectSO();

                    if (GetFryingReciteSOWithInput(inputKitchenObjectSO) != null)
                    {
                        kitchenObject.SetKitchenObjectParent(this);
                        InteractLogicPlaceObjectOnCounterServerRpc(
                            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO())
                        );
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

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
        {
            fryingTimer.Value = 0f;
            CounterState.Value = State.Frying;
            SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
        }

        [ClientRpc]
        private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
        {
            KitchenObjectSO kitchenObjectSOInput =
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
            fryingRecipeSO = GetFryingReciteSOWithInput(kitchenObjectSOInput);
        }

        [ClientRpc]
        private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
        {
            KitchenObjectSO kitchenObjectSOInput =
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
            burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSOInput);
        }

        private void ResetTheStoveCounter()
        {
            CounterState.Value = State.Idle;
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