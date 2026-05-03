using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundedGravity = -2f;

    [Header("References")]
    public Animator animator;

    private CharacterController characterController;
    private Vector3 verticalVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
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
            inputDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            Vector3 moveDirection = inputDirection * currentSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
        }

        ApplyGravity();
        UpdateAnimator(hasMovementInput, isRunning);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = groundedGravity;
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
