using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f,
                  rotateSpeed = 10f,
                  playerRadius = .7f,
                  playerHeight = 2f;

    [SerializeField]
    private GameInput gameInput;
    private bool isWalking;

    private void Update()
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

    public bool IsWalking()
    {
        return isWalking;
    }
}
