using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayDurationInSeconds = 60f;

    [Header("Sun Settings")]
    public Light sunLight;
    public Transform visibleSun;
    public float minSunIntensity = 0.05f;
    public float maxSunIntensity = 1.2f;

    [Header("Sun Rotation")]
    public float sunYRotation = 170f;

    [Header("Visible Sun Settings")]
    public Vector3 orbitCenter = new Vector3(80f, 0f, 80f);
    public float orbitRadius = 120f;
    public float orbitHeight = 60f;

    private float currentTime = 0f;

    void Update()
    {
        if (sunLight == null)
        {
            return;
        }

        currentTime += Time.deltaTime;

        if (currentTime > dayDurationInSeconds)
        {
            currentTime = 0f;
        }

        float dayProgress = currentTime / dayDurationInSeconds;

        RotateSunLight(dayProgress);
        UpdateSunIntensity(dayProgress);
        MoveVisibleSun(dayProgress);
    }

    private void RotateSunLight(float dayProgress)
    {
        float sunXRotation = dayProgress * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunXRotation, sunYRotation, 0f);
    }

    private void UpdateSunIntensity(float dayProgress)
    {
        float intensityFactor = Mathf.Sin(dayProgress * Mathf.PI);
        sunLight.intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, intensityFactor);
    }

    private void MoveVisibleSun(float dayProgress)
    {
        if (visibleSun == null)
        {
            return;
        }

        float angle = dayProgress * Mathf.PI * 2f;

        float x = orbitCenter.x + Mathf.Cos(angle) * orbitRadius;
        float y = orbitCenter.y + Mathf.Sin(angle) * orbitHeight + orbitHeight;
        float z = orbitCenter.z;

        visibleSun.position = new Vector3(x, y, z);
    }
}
