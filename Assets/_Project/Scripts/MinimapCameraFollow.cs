using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform target;

    [Header("Camera Settings")]
    public float height = 80f;
    public bool rotateWithTarget = true;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 newPosition = target.position;
        newPosition.y = height;
        transform.position = newPosition;

        if (rotateWithTarget)
        {
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
