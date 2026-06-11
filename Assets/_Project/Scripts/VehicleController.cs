using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float reverseAcceleration = 20f;
    [SerializeField] private float maxSpeed = 40f;
    [SerializeField] private float steeringSpeed = 100f;

    [Header("State")]
    [SerializeField] private bool canDrive = false;

    private Rigidbody rb;
    private float moveInput;
    private float steerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!canDrive)
        {
            moveInput = 0f;
            steerInput = 0f;
            return;
        }

        moveInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (!canDrive)
            return;

        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        float appliedAcceleration =
            moveInput >= 0f ? acceleration : reverseAcceleration;

        bool isBelowMaxSpeed = Mathf.Abs(currentSpeed) < maxSpeed;
        bool isChangingDirection =
            Mathf.Abs(moveInput) > 0.01f &&
            Mathf.Sign(moveInput) != Mathf.Sign(currentSpeed);

        if (isBelowMaxSpeed || isChangingDirection)
        {
            rb.AddForce(
                transform.forward * moveInput * appliedAcceleration,
                ForceMode.Acceleration
            );
        }

        if (Mathf.Abs(currentSpeed) > 0.2f)
        {
            float movementDirection = Mathf.Sign(currentSpeed);

            Quaternion turnRotation = Quaternion.Euler(
                0f,
                steerInput * steeringSpeed * movementDirection * Time.fixedDeltaTime,
                0f
            );

            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    public void SetDriving(bool driving)
    {
        canDrive = driving;

        if (!driving)
        {
            moveInput = 0f;
            steerInput = 0f;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public bool IsDriving()
    {
        return canDrive;
    }
}