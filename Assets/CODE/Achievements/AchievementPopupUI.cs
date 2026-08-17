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
        // Wait 4 seconds before showing
        yield return new WaitForSeconds(4f);

        panel.SetActive(true);

        // Play achievement sound
        if (audioSource != null && achievementSound != null)
        {
            audioSource.PlayOneShot(achievementSound);
        }

        // Keep popup visible for 4 seconds
        yield return new WaitForSeconds(4f);

        panel.SetActive(false);
    }
}