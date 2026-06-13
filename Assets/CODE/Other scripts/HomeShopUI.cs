using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HomeShopUI : MonoBehaviour
{
    [Header("Buttons")]
    public TMP_Text speedBoostButtonText;
    public TMP_Text jumpBoostButtonText;

    [Header("Coins")]
    public TMP_Text coinsText;

    [Header("Not Enough Coins Popup")]
    public GameObject notEnoughCoinsPopup;

    int speedBoostCost = 0;
    int jumpBoostCost = 25;

    public GameObject firstSelectedButton;

    void Start()
    {
        if (notEnoughCoinsPopup != null)
        {
            notEnoughCoinsPopup.SetActive(false);
        }

        UpdateUI();
    }

    

    // =========================
    // SPEED BOOST
    // =========================

    public void BuySpeedBoost()
    {
        // ALREADY OWNED
        if (UpgradeData.ownsSpeedBoost)
        {
            EquipSpeedBoost();
            return;
        }

        // NOT ENOUGH MONEY
        if (UpgradeData.totalCoins < speedBoostCost)
        {
            ShowNotEnoughCoins();
            return;
        }

        // BUY
        UpgradeData.totalCoins -= speedBoostCost;

        UpgradeData.ownsSpeedBoost = true;

        UpgradeData.equippedUpgrade =
            "SpeedBoost";

        UpdateUI();

        Debug.Log("Bought Speed Boost");
    }

    public void EquipSpeedBoost()
    {
        if (!UpgradeData.ownsSpeedBoost)
            return;

        UpgradeData.equippedUpgrade =
            "SpeedBoost";

        UpdateUI();

        Debug.Log("Equipped Speed Boost");
    }

    // =========================
    // JUMP BOOST
    // =========================

    public void BuyJumpBoost()
    {
        // ALREADY OWNED
        if (UpgradeData.ownsJumpBoost)
        {
            EquipJumpBoost();
            return;
        }

        // NOT ENOUGH MONEY
        if (UpgradeData.totalCoins < jumpBoostCost)
        {
            ShowNotEnoughCoins();
            return;
        }

        // BUY
        UpgradeData.totalCoins -= jumpBoostCost;

        UpgradeData.ownsJumpBoost = true;

        UpgradeData.equippedUpgrade =
            "JumpBoost";

        UpdateUI();

        Debug.Log("Bought Jump Boost");
    }

    public void EquipJumpBoost()
    {
        if (!UpgradeData.ownsJumpBoost)
            return;

        UpgradeData.equippedUpgrade =
            "JumpBoost";

        UpdateUI();

        Debug.Log("Equipped Jump Boost");
    }

    // =========================
    // POPUP
    // =========================

    void ShowNotEnoughCoins()
    {
        if (notEnoughCoinsPopup != null)
        {
            notEnoughCoinsPopup.SetActive(true);
        }
    }

    public void CloseNotEnoughCoinsPopup()
    {
        if (notEnoughCoinsPopup != null)
        {
            notEnoughCoinsPopup.SetActive(false);
        }
    }

    // =========================
    // UI
    // =========================

    void UpdateUI()
    {
        // TOTAL COINS
        if (coinsText != null)
        {
            coinsText.text =
                UpgradeData.totalCoins.ToString();
        }

        // =========================
        // SPEED BOOST BUTTON
        // =========================

        if (UpgradeData.equippedUpgrade ==
            "SpeedBoost")
        {
            speedBoostButtonText.text =
                "Equipped";
        }
        else if (UpgradeData.ownsSpeedBoost)
        {
            speedBoostButtonText.text =
                "Owned";
        }
        else
        {
            speedBoostButtonText.text =
                speedBoostCost.ToString();
        }

        // =========================
        // JUMP BOOST BUTTON
        // =========================

        if (UpgradeData.equippedUpgrade ==
            "JumpBoost")
        {
            jumpBoostButtonText.text =
                "Equipped";
        }
        else if (UpgradeData.ownsJumpBoost)
        {
            jumpBoostButtonText.text =
                "Owned";
        }
        else
        {
            jumpBoostButtonText.text =
                jumpBoostCost.ToString();
        }
    }

    public void SelectFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public bool IsPopupOpen()
    {
        return notEnoughCoinsPopup != null &&
               notEnoughCoinsPopup.activeSelf;
    }


}