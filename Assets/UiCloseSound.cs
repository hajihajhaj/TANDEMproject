using UnityEngine;

public class UiCloseSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip closeSound;

    public void PlayCloseSound()
    {
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}
