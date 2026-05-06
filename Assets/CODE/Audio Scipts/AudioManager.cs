using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxSource;
    public AudioClip buttonClick;

    void Awake()
    {
        instance = this;
    }

    public void PlayClick()
    {
        sfxSource.PlayOneShot(buttonClick);
    }
}
