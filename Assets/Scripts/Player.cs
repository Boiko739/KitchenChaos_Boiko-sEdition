using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
        public OnSelectedCounterChangedEventArgs(BaseCounter counter)
        {
            selectedCounter = counter;
        }
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterlayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private readonly float rotateSpeed = 10f,
                           playerRadius = .7f,
                           interactDistance = 2f,
                           moveSpeed = 7f,
                           playerHeight = 2f;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance");
        }

        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
        gameInput.OnInteractAlternateAction+= GameInputOnInteractAlternateAction;
    }

    private void GameInputOnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterlayerMask)
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
        var inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDir, moveDistance);
        if (canMove)
        {
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }
        else
        {
            //Attempt to move only X or Z direction

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                transform.position += moveSpeed * Time.deltaTime * moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirZ, moveDistance);
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

        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
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
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
