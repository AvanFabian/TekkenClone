using UnityEngine;

public class ButtonSoundController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        // Automatically get the AudioSource component on this GameObject
        audioSource = GetComponent<AudioSource>();

        // Log an error if AudioSource is missing
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on " + gameObject.name + ". Please add one.");
        }
    }

    // Method to play hover sound
    public void PlayHoverSound()
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
        else
        {
            Debug.LogWarning("Hover sound or AudioSource is missing!");
        }
    }

    // Method to play click sound
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Click sound or AudioSource is missing!");
        }
    }
}
