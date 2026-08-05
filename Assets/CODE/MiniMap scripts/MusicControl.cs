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


    bool paused;


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


    void OnApplicationPause(bool pause)
    {
        paused = pause;


        if (pause)
            musicSource.Pause();
        else
            musicSource.UnPause();
    }



    IEnumerator RadioLoop()
    {
        while (true)
        {
            // Play song
            musicSource.clip = songs[songIndex];
            musicSource.Play();


            // Wait for song
            yield return new WaitWhile(() => musicSource.isPlaying);



            // Move to next song
            songIndex++;

            if (songIndex >= songs.Length)
                songIndex = 0;



            // Small transition
            yield return new WaitForSeconds(0.5f);



            // Always play radio after song
            yield return StartCoroutine(PlayRandomRadio());


            // Small transition before next song
            yield return new WaitForSeconds(0.5f);
        }
    }



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



        yield return new WaitWhile(() => musicSource.isPlaying);
    }



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