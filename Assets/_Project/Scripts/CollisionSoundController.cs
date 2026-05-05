using UnityEngine;

public class CollisionSoundController : MonoBehaviour
{
    [Header("References")]
    public AudioSource collisionAudioSource;

    [Header("Collision Settings")]
    public float minimumTimeBetweenSounds = 0.5f;

    private float lastSoundTime = -999f;

    void Start()
    {
        if (collisionAudioSource == null)
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();

            if (audioSources.Length > 1)
            {
                collisionAudioSource = audioSources[audioSources.Length - 1];
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (collisionAudioSource == null)
        {
            return;
        }

        if (Time.time - lastSoundTime < minimumTimeBetweenSounds)
        {
            return;
        }

        collisionAudioSource.Play();
        lastSoundTime = Time.time;
    }
}
