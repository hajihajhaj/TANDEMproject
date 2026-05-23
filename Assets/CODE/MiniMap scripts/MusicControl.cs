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

    [Header("Delivery Timer")]
    public DeliveryManager deliveryManager;

    void Start()
    {
        musicSource.volume = 1f;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = musicSource.volume;

        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.up.wasPressedThisFrame)
            {
                VolumeUp();
            }

            if (Gamepad.current.dpad.down.wasPressedThisFrame)
            {
                VolumeDown();
            }
        }

        UpdateMusicSpeed();
    }

    void UpdateMusicSpeed()
    {
        if (deliveryManager == null)
            return;

        float percent = deliveryManager.GetRemainingTimePercent();

        float targetPitch = 1f;

        // SLIGHT TENSION
        if (percent <= 0.50f)
        {
            targetPitch = 1.03f;
        }

        // MEDIUM PANIC
        if (percent <= 0.25f)
        {
            targetPitch = 1.08f;
        }

        // FINAL PANIC
        if (percent <= 0.10f)
        {
            targetPitch = 1.15f;
        }

        musicSource.pitch = Mathf.Lerp(
            musicSource.pitch,
            targetPitch,
            Time.deltaTime * 2f
        );
    }

    public void ChangeVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void VolumeUp()
    {
        musicSource.volume = Mathf.Clamp01(musicSource.volume + volumeStep);
        volumeSlider.value = musicSource.volume;
    }

    public void VolumeDown()
    {
        musicSource.volume = Mathf.Clamp01(musicSource.volume - volumeStep);
        volumeSlider.value = musicSource.volume;
    }
}