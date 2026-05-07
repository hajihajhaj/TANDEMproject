using UnityEngine;
using TMPro;

public class HomeShopUI : MonoBehaviour
{
    public TMP_Text speedBoostButtonText;
    public TMP_Text jumpBoostButtonText;

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

    public void EquipSpeedBoost()
    {
        if (!UpgradeData.ownsSpeedBoost)
            return;

        UpgradeData.equippedUpgrade = "SpeedBoost";

        UpdateUI();
    }

    public void BuyJumpBoost()
    {
        Debug.Log("HOME SHOP JUMP BUTTON");

        UpgradeData.ownsJumpBoost = true;

        EquipJumpBoost();

        UpdateUI();
    }

    public void EquipJumpBoost()
    {
        if (!UpgradeData.ownsJumpBoost)
            return;

        UpgradeData.equippedUpgrade = "JumpBoost";

        UpdateUI();
    }

    void UpdateUI()
    {
        // SPEED BOOST BUTTON
        if (UpgradeData.equippedUpgrade == "SpeedBoost")
        {
            speedBoostButtonText.text = "Owned";
        }
        else
        {
            speedBoostButtonText.text = "Speed Boost";
        }

        // JUMP BOOST BUTTON
        if (UpgradeData.equippedUpgrade == "JumpBoost")
        {
            jumpBoostButtonText.text = "Owned";
        }
        else
        {
            jumpBoostButtonText.text = "Jump Boost";
        }
    }
}