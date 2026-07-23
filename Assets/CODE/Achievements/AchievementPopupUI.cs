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
        panel.SetActive(true);

        // Play sound
        if (audioSource != null && achievementSound != null)
        {
            audioSource.PlayOneShot(achievementSound);
        }

        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(3f);
        panel.SetActive(false);
    }
}