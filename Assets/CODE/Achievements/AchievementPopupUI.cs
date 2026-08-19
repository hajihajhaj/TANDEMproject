using UnityEngine;
using System.Collections;

public class AchievementPopupUI : MonoBehaviour
{
    public static AchievementPopupUI Instance;

    public GameObject panel;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip achievementSound;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show()
    {
        StartCoroutine(ShowDelayed());
    }

    IEnumerator ShowDelayed()
    {
        // Show achievement immediately
        // after the thank-you message finishes
        panel.SetActive(true);

        // Play achievement sound
        if (audioSource != null && achievementSound != null)
        {
            audioSource.PlayOneShot(achievementSound);
        }

        // Keep popup visible for 4 seconds
        yield return new WaitForSeconds(4f);

        panel.SetActive(false);

        // Show level summary after achievement
        if (DeliveryManager.instance != null)
        {
            DeliveryManager.instance.ShowSummaryAfterAchievement();
        }
    }
}