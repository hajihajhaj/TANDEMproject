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
        deliveries++;

        Debug.Log("Deliveries completed: " + deliveries);
    }

    public void ResetDeliveries()
    {
        deliveries = 0;

        Debug.Log("Achievement deliveries reset.");
    }

    public void UnlockAchievement()
    {
        // Already unlocked this achievement
        if (unlocked)
        {
            Debug.Log("Achievement was already unlocked.");
            return;
        }

        unlocked = true;

        unlockDate =
            System.DateTime.Now.ToString("MMM dd");

        Debug.Log(
            "Achievement Unlocked: " +
            unlockDate
        );
    }
}