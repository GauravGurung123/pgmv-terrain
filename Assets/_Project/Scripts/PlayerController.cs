using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 12f;

    [Header("Jump")]
    public float jumpHeight = 2f;

    [Header("Gravity")]
    public float gravity = -20f;
    public float groundedGravity = -2f;

    [Header("References")]
    public Animator animator;
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 verticalVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);
        bool hasMovementInput = inputDirection.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (hasMovementInput)
        {
            Vector3 moveDirection = GetCameraRelativeMoveDirection(inputDirection);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

        HandleJumpAndGravity(hasMovementInput, isRunning);
        UpdateAnimator(hasMovementInput, isRunning);
    }

    private Vector3 GetCameraRelativeMoveDirection(Vector3 inputDirection)
    {
        if (cameraTransform == null)
        {
            inputDirection.Normalize();
            return inputDirection;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
        moveDirection.Normalize();

        return moveDirection;
    }

    private void HandleJumpAndGravity(bool hasMovementInput, bool isRunning)
    {
        bool grounded = characterController.isGrounded;

        if (grounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = groundedGravity;
        }

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                if (hasMovementInput && isRunning)
                {
                    animator.SetTrigger("RunJump");
                }
                else
                {
                    animator.SetTrigger("Jump");
                }
            }
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private void UpdateAnimator(bool hasMovementInput, bool isRunning)
    {
        if (animator == null)
        {
            return;
        }

        float animationSpeed = 0f;

        if (hasMovementInput)
        {
            animationSpeed = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat("Speed", animationSpeed);
    }
}
