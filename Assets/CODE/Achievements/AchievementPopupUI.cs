using UnityEngine;
using System.Collections;

public class AchievementPopupUI : MonoBehaviour
{
    public static AchievementPopupUI Instance;

    public GameObject panel;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip achievementSound;

    bool isShowing = false;

    void Awake()
    {
        Instance = this;

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void Show()
    {
        // Don't start the popup more than once
        if (isShowing)
            return;

        StartCoroutine(ShowDelayed());
    }

    IEnumerator ShowDelayed()
    {
        isShowing = true;

        // Show achievement
        if (panel != null)
        {
            panel.SetActive(true);
        }

        // Play achievement sound
        if (
            audioSource != null &&
            achievementSound != null
        )
        {
            audioSource.PlayOneShot(
                achievementSound
            );
        }

        Debug.Log("Achievement popup shown.");

        // Keep achievement visible for 4 seconds
        yield return new WaitForSeconds(4f);

        // Hide achievement
        if (panel != null)
        {
            panel.SetActive(false);
        }

        isShowing = false;

        Debug.Log("Achievement popup closed.");

        // Now show the level summary
        if (DeliveryManager.instance != null)
        {
            DeliveryManager.instance.ShowSummaryAfterAchievement();
        }
    }
}