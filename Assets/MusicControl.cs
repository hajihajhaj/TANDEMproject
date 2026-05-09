using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MusicControl : MonoBehaviour
{
    [Header("Music")]
    public AudioSource musicSource;

    [Header("Volume UI")]
    public Slider volumeSlider;

    [Header("Controller Volume")]
    public float volumeStep = 0.1f;

    void Start()
    {
        // Start volume
        musicSource.volume = 1f;

        // Slider setup
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = musicSource.volume;

        // Listen for slider movement
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    void Update()
    {
        // PS5 / controller D-pad controls
        if (Gamepad.current != null)
        {
            // Volume Up
            if (Gamepad.current.dpad.up.wasPressedThisFrame)
            {
                VolumeUp();
            }

            // Volume Down
            if (Gamepad.current.dpad.down.wasPressedThisFrame)
            {
                VolumeDown();
            }
        }
    }

    public void ChangeVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void VolumeUp()
    {
        musicSource.volume = Mathf.Clamp01(musicSource.volume + volumeStep);

        // Update slider visually
        volumeSlider.value = musicSource.volume;
    }

    public void VolumeDown()
    {
        musicSource.volume = Mathf.Clamp01(musicSource.volume - volumeStep);

        // Update slider visually
        volumeSlider.value = musicSource.volume;
    }
}