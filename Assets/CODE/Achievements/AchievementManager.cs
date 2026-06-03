using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    public bool unlocked = false;
    public int deliveries = 0;
    public string unlockDate = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDelivery()
    {
        if (unlocked) return;

        deliveries++;

        if (deliveries >= 3)
        {
            UnlockAchievement();
        }
    }

    void UnlockAchievement()
    {
        unlocked = true;

        // ONLY month + day
        unlockDate = System.DateTime.Now.ToString("MMM dd");

        Debug.Log("Achievement Unlocked: " + unlockDate);

        if (AchievementPopupUI.Instance != null)
            AchievementPopupUI.Instance.Show();
    }
}