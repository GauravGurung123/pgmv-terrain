using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public TMP_Text speedText;
    public TMP_Text headingText;

    private Vector3 lastPlayerPosition;

    void Start()
    {
        if (player != null)
        {
            lastPlayerPosition = player.position;
        }
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        UpdateSpeed();
        UpdateHeading();

        lastPlayerPosition = player.position;
    }

    private void UpdateSpeed()
    {
        float distanceMoved = Vector3.Distance(player.position, lastPlayerPosition);
        float speed = distanceMoved / Time.deltaTime;

        if (speedText != null)
        {
            speedText.text = "Speed: " + speed.ToString("F1") + " m/s";
        }
    }

    private void UpdateHeading()
    {
        float heading = player.eulerAngles.y;

        if (headingText != null)
        {
            headingText.text = "Heading: " + heading.ToString("F0") + "°";
        }
    }
}
