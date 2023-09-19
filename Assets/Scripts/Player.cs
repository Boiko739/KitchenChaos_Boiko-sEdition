using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f;
    [SerializeField]
    private GameInput gameInput;
    [SerializeField]
    private LayerMask counterlayerMask;
    private readonly float rotateSpeed = 10f,
     playerRadius = .7f,
     interactDistance = 2f,
     playerHeight = 2f;

    private bool isWalking;
    private Vector3 lastInteractDir;

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

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterlayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
        else
        {
            print("-");
        }

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
            canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                transform.position += moveSpeed * Time.deltaTime * moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirZ, moveDistance);
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
}
