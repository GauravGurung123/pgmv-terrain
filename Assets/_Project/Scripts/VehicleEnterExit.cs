using UnityEngine;
using Unity.Cinemachine;

public class VehicleEnterExit : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private VehicleController vehicleController;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform playerCameraTarget;
    [SerializeField] private Transform vehicleCameraTarget;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private bool playerNearby;
    private bool isDriving;

    private void Update()
    {
        if (!playerNearby && !isDriving)
            return;

        if (Input.GetKeyDown(interactionKey))
        {
            if (isDriving)
                ExitVehicle();
            else
                EnterVehicle();
        }
    }

    private void EnterVehicle()
    {
        isDriving = true;
        vehicleController.SetDriving(true);

        cinemachineCamera.Target.TrackingTarget = vehicleCameraTarget;

        player.SetActive(false);
    }

    private void ExitVehicle()
    {
        isDriving = false;
        vehicleController.SetDriving(false);

        player.transform.position = exitPoint.position;
        player.transform.rotation = exitPoint.rotation;
        player.SetActive(true);

        cinemachineCamera.Target.TrackingTarget = playerCameraTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            playerNearby = false;
    }
}