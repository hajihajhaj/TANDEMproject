using UnityEngine;

public class NotificationSoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Notification Sounds")]
    public AudioClip defaultSound;
    public AudioClip notificationSound1;
    public AudioClip notificationSound2;

    public static AudioClip CurrentNotificationSound;

    void Start()
    {
        LoadNotificationSound();
    }

    public void SetDefault()
    {
        CurrentNotificationSound = defaultSound;

        audioSource.PlayOneShot(defaultSound);

        PlayerPrefs.SetInt("NotificationSound", 0);
        PlayerPrefs.Save();
    }

    public void SetNotificationSound1()
    {
        CurrentNotificationSound = notificationSound1;

        audioSource.PlayOneShot(notificationSound1);

        PlayerPrefs.SetInt("NotificationSound", 1);
        PlayerPrefs.Save();
    }

    public void SetNotificationSound2()
    {
        CurrentNotificationSound = notificationSound2;

        audioSource.PlayOneShot(notificationSound2);

        PlayerPrefs.SetInt("NotificationSound", 2);
        PlayerPrefs.Save();
    }

    void LoadNotificationSound()
    {
        int sound = PlayerPrefs.GetInt("NotificationSound", 0);

        switch (sound)
        {
            case 0:
                CurrentNotificationSound = defaultSound;
                break;

            case 1:
                CurrentNotificationSound = notificationSound1;
                break;

            case 2:
                CurrentNotificationSound = notificationSound2;
                break;
        }
    }
}