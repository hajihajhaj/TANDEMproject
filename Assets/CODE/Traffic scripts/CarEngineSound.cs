using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarEngineSound : MonoBehaviour
{
    [Header("Engine Sound")]
    public AudioClip engineLoop;

    [Range(0f, 1f)]
    public float volume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = engineLoop;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.volume = volume;

        if (engineLoop != null)
        {
            audioSource.Play();
        }
    }
}