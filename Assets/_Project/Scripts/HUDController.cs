using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform vehicle;
    [SerializeField] private Rigidbody vehicleRigidbody;
    [SerializeField] private VehicleController vehicleController;

    [Header("UI")]
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text headingText;

    private Vector3 lastPlayerPosition;

    private void Start()
    {
        if (player != null)
        {
            lastPlayerPosition = player.position;
        }
    }

    private void Update()
    {
        bool driving = vehicleController != null && vehicleController.IsDriving();

        Transform currentTarget = driving ? vehicle : player;

        if (currentTarget == null)
            return;

        UpdateSpeed(driving);
        UpdateHeading(currentTarget);

        if (!driving && player != null)
        {
            lastPlayerPosition = player.position;
        }
    }

    private void UpdateSpeed(bool driving)
    {
        float speed;

        if (driving && vehicleRigidbody != null)
        {
            speed = vehicleRigidbody.linearVelocity.magnitude;
        }
        else
        {
            float distanceMoved = Vector3.Distance(player.position, lastPlayerPosition);
            speed = Time.deltaTime > 0f ? distanceMoved / Time.deltaTime : 0f;
        }

        if (speedText != null)
        {
            speedText.text = $"Speed: {speed:F1} m/s";
        }
    }

    private void UpdateHeading(Transform target)
    {
        float heading = target.eulerAngles.y;

        if (headingText != null)
        {
            headingText.text = $"Heading: {heading:F0}°";
        }
    }
}