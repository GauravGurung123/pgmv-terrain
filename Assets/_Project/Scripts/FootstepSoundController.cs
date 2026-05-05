using UnityEngine;

public class FootstepSoundController : MonoBehaviour
{
    [Header("References")]
    public AudioSource footstepAudioSource;

    [Header("Movement Settings")]
    public float minimumSpeedToPlay = 0.2f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (footstepAudioSource == null)
        {
            return;
        }

        float speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        bool isMoving = speed > minimumSpeedToPlay;

        if (isMoving && !footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
        else if (!isMoving && footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }

        lastPosition = transform.position;
    }
}
