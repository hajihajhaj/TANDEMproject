using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class MusicControl : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicSource;

    [Header("Songs (in order)")]
    public AudioClip[] songs;

    [Header("Advertisements")]
    public AudioClip[] commercials;

    [Header("DJ One Liners")]
    public AudioClip[] oneLiners;

    [Header("Conversations")]
    public AudioClip[] conversations;

    [Header("Volume")]
    public Slider volumeSlider;
    public float volumeStep = 0.1f;

    [Header("Delivery")]
    public DeliveryManager deliveryManager;

    int songIndex = 0;

    int lastCategory = -1;
    AudioClip lastClip;

    bool paused = false;


    void Start()
    {
        musicSource.volume = 1;

        if (volumeSlider)
        {
            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 1;
            volumeSlider.value = 1;

            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }

        StartCoroutine(RadioLoop());
    }


    void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.up.wasPressedThisFrame)
                VolumeUp();

            if (Gamepad.current.dpad.down.wasPressedThisFrame)
                VolumeDown();
        }

        UpdateMusicSpeed();
    }


    // =========================
    // PAUSE STATE
    // =========================

    public void SetPaused(bool value)
    {
        paused = value;
    }


    void OnApplicationPause(bool pause)
    {
        paused = pause;
    }


    // =========================
    // RADIO LOOP
    // =========================

    IEnumerator RadioLoop()
    {
        while (true)
        {
            // Wait while the game is paused
            yield return new WaitWhile(() => paused);


            // Play song
            musicSource.clip = songs[songIndex];
            musicSource.Play();


            // Wait until the song finishes
            while (true)
            {
                // If paused, wait here and DO NOT advance
                if (paused)
                {
                    yield return new WaitWhile(() => paused);
                    continue;
                }

                // If the song is still playing, keep waiting
                if (musicSource.isPlaying)
                {
                    yield return null;
                    continue;
                }

                // Song really finished
                break;
            }


            // Move to next song
            songIndex++;

            if (songIndex >= songs.Length)
                songIndex = 0;


            // Small transition
            yield return new WaitForSeconds(0.5f);


            // Wait if paused
            yield return new WaitWhile(() => paused);


            // Play radio
            yield return StartCoroutine(PlayRandomRadio());


            // Small transition
            yield return new WaitForSeconds(0.5f);
        }
    }


    // =========================
    // RANDOM RADIO
    // =========================

    IEnumerator PlayRandomRadio()
    {
        int category;


        do
        {
            category = Random.Range(0, 3);

        } while (category == lastCategory);


        lastCategory = category;


        AudioClip[] clips;


        if (category == 0)
            clips = commercials;

        else if (category == 1)
            clips = oneLiners;

        else
            clips = conversations;


        if (clips.Length == 0)
            yield break;


        AudioClip chosen;


        do
        {
            chosen = clips[Random.Range(0, clips.Length)];

        } while (clips.Length > 1 && chosen == lastClip);


        lastClip = chosen;


        musicSource.clip = chosen;
        musicSource.Play();


        // Wait until the radio clip ACTUALLY finishes
        while (true)
        {
            // If paused, wait here
            if (paused)
            {
                yield return new WaitWhile(() => paused);
                continue;
            }

            // Still playing
            if (musicSource.isPlaying)
            {
                yield return null;
                continue;
            }

            // Really finished
            break;
        }
    }


    // =========================
    // MUSIC SPEED
    // =========================

    void UpdateMusicSpeed()
    {
        if (!deliveryManager)
            return;


        float percent = deliveryManager.GetRemainingTimePercent();

        float target = 1f;


        if (percent <= 0.5f)
            target = 1.03f;


        if (percent <= 0.25f)
            target = 1.08f;


        if (percent <= 0.10f)
            target = 1.15f;


        musicSource.pitch = Mathf.Lerp(
            musicSource.pitch,
            target,
            Time.deltaTime * 2f
        );
    }


    // =========================
    // VOLUME
    // =========================

    public void ChangeVolume(float value)
    {
        musicSource.volume = value;
    }


    public void VolumeUp()
    {
        musicSource.volume = Mathf.Clamp01(
            musicSource.volume + volumeStep
        );


        if (volumeSlider)
            volumeSlider.value = musicSource.volume;
    }


    public void VolumeDown()
    {
        musicSource.volume = Mathf.Clamp01(
            musicSource.volume - volumeStep
        );


        if (volumeSlider)
            volumeSlider.value = musicSource.volume;
    }
}