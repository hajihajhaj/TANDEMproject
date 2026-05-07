using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("References")]
    public TandemBikeController bikeMovement;

    [Header("UI")]
    public TMP_Text upgradeNameText;
    public TMP_Text cooldownText;

    [Header("Speed Boost")]
    public bool ownsSpeedBoost;
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 3f;
    public float speedBoostCooldown = 10f;
    public float boostForce = 120f;
    public float boostDuration = 2f;

    private bool canUseAbility = true;
    private bool speedBoostEquipped;

    private float cooldownRemaining;

    void Start()
    {
        Debug.Log("Current Equipped Upgrade: " +
            UpgradeData.equippedUpgrade);
    }

    void Update()
    {
        HandleAbilityInput();
        UpdateCooldownUI();
    }

    void HandleAbilityInput()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            // STOP if cooldown active
            if (!canUseAbility)
                return;

            // STOP if no upgrade equipped
            if (UpgradeData.equippedUpgrade != "SpeedBoost")
                return;

            StartCoroutine(ActivateSpeedBoost());
        }
    }

    IEnumerator ActivateSpeedBoost()
    {
        canUseAbility = false;
        bikeMovement.isBoosting = true;
        float timer = 0f;

        while (timer < boostDuration)
        {
            Vector3 boostVelocity =
                bikeMovement.transform.forward * boostForce;

            bikeMovement.rb.linearVelocity = new Vector3(
                boostVelocity.x,
                bikeMovement.rb.linearVelocity.y,
                boostVelocity.z
            );

            timer += Time.deltaTime;

            yield return null;
        }
        bikeMovement.isBoosting = false;
        cooldownRemaining = speedBoostCooldown;

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }

        cooldownRemaining = 0;
        canUseAbility = true;
    }

    void UpdateCooldownUI()
    {
        if (UpgradeData.equippedUpgrade == "SpeedBoost")
        {
            upgradeNameText.text = "Speed Boost";

            if (canUseAbility)
            {
                cooldownText.text = "Ready";
            }
            else
            {
                cooldownText.text =
                    Mathf.Ceil(cooldownRemaining).ToString();
            }
        }
        else
        {
            upgradeNameText.text = "No Upgrade Equipped";
            cooldownText.text = "";
        }
    }

    // PURCHASE
    public void BuySpeedBoost()
    {
        ownsSpeedBoost = true;

        EquipSpeedBoost();
    }

    // EQUIP
    public void EquipSpeedBoost()
    {
        if (!ownsSpeedBoost)
            return;

        // UNEQUIP EVERYTHING
        speedBoostEquipped = false;

        // EQUIP THIS
        speedBoostEquipped = true;
    }
}