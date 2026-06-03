using UnityEngine;
using TMPro;

public class AchievementMenuUI : MonoBehaviour
{
    public GameObject lockedIcon;
    public GameObject unlockedIcon;
    public TMP_Text dateText;

    void Update()
    {
        if (AchievementManager.Instance == null) return;

        var a = AchievementManager.Instance;

        if (a.unlocked)
        {
            lockedIcon.SetActive(false);
            unlockedIcon.SetActive(true);

            dateText.text = "Unlocked: " + a.unlockDate;
        }
        else
        {
            lockedIcon.SetActive(true);
            unlockedIcon.SetActive(false);

            dateText.text = "Locked";
        }
    }
}
