using Counters;
using OtherScripts;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos
{
    public class Player : NetworkBehaviour, IKitchenObjectParent
    {
        public static Player LocalInstance { get; private set; }


        public static event EventHandler OnAnyPlayerSpawned;

        public static event EventHandler OnAnyPlayerPickedSomething;

        public static event EventHandler OnAnyPlayerWalking;

        public static void ResetStaticData()
        {
            OnAnyPlayerSpawned = null;
            OnAnyPlayerPickedSomething = null;
            OnAnyPlayerWalking = null;
        }

        public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

        public class OnSelectedCounterChangedEventArgs : EventArgs
        {
            public BaseCounter SelectedCounter { get; private set; }

            public OnSelectedCounterChangedEventArgs(BaseCounter counter)
            {
                SelectedCounter = counter;
            }
        }

        [SerializeField] private LayerMask counterLayerMask;
        [SerializeField] private LayerMask collisionsLayerMask;
        [SerializeField] private Transform kitchenObjectHoldPoint;
        [SerializeField] private List<Vector3> spawnPositionList;
        [SerializeField] private PlayerVisual playerVisual;

        private readonly float rotateSpeed = 10f,
                               playerRadius = .7f,
                               interactDistance = 2f,
                               moveSpeed = 7f,
                               footstepTimerMax = .1f;

        private float footstepTimer;

        public bool IsWalking { get; private set; }

        private Vector3 lastInteractDir;
        private BaseCounter selectedCounter;
        private KitchenObject kitchenObject;

        private void Start()
        {
            GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
            GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromLocalId(OwnerClientId);
            playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                LocalInstance = this;
            }

            transform.position = spawnPositionList[KitchenGameMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];
            OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            }
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if (clientId == OwnerClientId && HasKitchenObject())
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
            }
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            HandleMovement();
            HandleInteractions();
        }

        private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
        {
            if (!GameManager.Instance.IsGamePlaying())
            {
                return;
            }

            if (selectedCounter != null)
            {
                selectedCounter.InteractAlternate(this);
            }
        }

        private void GameInput_OnInteractAction(object sender, EventArgs e)
        {
            if (!GameManager.Instance.IsGamePlaying())
            {
                return;
            }

            if (selectedCounter != null)
            {
                selectedCounter.Interact(this);
            }
        }

        private void HandleInteractions()
        {
            var inputVector = GameInput.Instance.GetMovementVectorNormalized();
            Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
            if (moveDir != Vector3.zero)
            {
                lastInteractDir = moveDir;
            }

            if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)
                && raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                if (counter != selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }

        private void SetSelectedCounter(BaseCounter clearCounter)
        {
            selectedCounter = clearCounter;

            OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs(selectedCounter));
        }

        private void HandleMovement()
        {
            var inputVector = GameInput.Instance.GetMovementVectorNormalized();
            Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
            float moveDistance = moveSpeed * Time.deltaTime;

            bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionsLayerMask);
            if (canMove)
            {
                transform.position += moveSpeed * Time.deltaTime * moveDir;
            }
            else
            {
                //Attempt to move only X or Z direction

                Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                canMove = (moveDir.x < -.5f || moveDir.x > .5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, collisionsLayerMask);
                if (canMove)
                {
                    transform.position += moveSpeed * Time.deltaTime * moveDirX;
                }
                else
                {
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = (moveDir.z < -.5f || moveDir.z > .5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, collisionsLayerMask);
                    if (canMove)
                    {
                        transform.position += moveSpeed * Time.deltaTime * moveDirZ;
                    }
                    else
                    {
                        //Cannot move anywhere
                    }
                }
            }

            IsWalking = moveDir != Vector3.zero;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
            if (IsWalking)
            {
                HandleWalkingSound();
            }
        }

        private void HandleWalkingSound()
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepTimerMax)
            {
                footstepTimer = 0f;
                OnAnyPlayerWalking?.Invoke(this, EventArgs.Empty);
            }
        }

        public Transform GetKitchenObjectFollowTransform()
        {
            return kitchenObjectHoldPoint;
        }

        public KitchenObject GetKitchenObject()
        {
            return kitchenObject;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;

            if (kitchenObject != null)
            {
                OnAnyPlayerPickedSomething?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ClearKitchenObject()
        {
            kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return kitchenObject != null;
        }

        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
        }
    }
}