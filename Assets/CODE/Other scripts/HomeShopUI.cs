using UnityEngine;
using TMPro;

public class HomeShopUI : MonoBehaviour
{
    public TMP_Text speedBoostButtonText;

    void Start()
    {
        UpdateUI();
    }

    public void BuySpeedBoost()
    {
        UpgradeData.ownsSpeedBoost = true;

        // IMPORTANT
        UpgradeData.equippedUpgrade = "SpeedBoost";

        UpdateUI();

        Debug.Log("Bought Speed Boost");
        Debug.Log("Equipped: " + UpgradeData.equippedUpgrade);
    }

    void UpdateUI()
    {
        if (UpgradeData.ownsSpeedBoost)
        {
            speedBoostButtonText.text = "Owned";
        }
        else
        {
            speedBoostButtonText.text = "Buy Speed Boost";
        }
    }
}