using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("References")]
    public TandemBikeController bikeMovement;

    [Header("UI")]
    public TMP_Text upgradeNameText;
    public TMP_Text cooldownText;

    // Upgrade Images
    public GameObject emptyUpgradeImage;
    public GameObject speedBoostImage;
    public GameObject jumpBoostImage;

    // Cooldown Overlay
    public Image cooldownOverlay;

    [Header("Speed Boost")]
    public bool ownsSpeedBoost;
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 3f;
    public float speedBoostCooldown = 10f;
    public float boostForce = 120f;
    public float boostDuration = 2f;

    private bool canUseAbility = true;

    private float cooldownRemaining;

    [Header("Jump Boost")]
    public float jumpForce = 55f;
    public float jumpCooldown = 6f;

    void Start()
    {
        Debug.Log("Current Equipped Upgrade: " +
            UpgradeData.equippedUpgrade);

        UpdateUpgradeImages();
    }

    void Update()
    {
        HandleAbilityInput();
        UpdateCooldownUI();
    }

    void HandleAbilityInput()
    {
        bool abilityPressed = false;

        // Keyboard Player 1
        if (Keyboard.current != null &&
            Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            abilityPressed = true;
        }

        // Controller Player 1 only
        if (Gamepad.all.Count > 0 &&
            Gamepad.all[0].buttonSouth.wasPressedThisFrame)
        {
            abilityPressed = true;
        }

        if (abilityPressed)
        {
            if (!canUseAbility)
                return;

            // SPEED BOOST
            if (UpgradeData.equippedUpgrade == "SpeedBoost")
            {
                StartCoroutine(ActivateSpeedBoost());
            }

            // JUMP BOOST
            else if (UpgradeData.equippedUpgrade == "JumpBoost")
            {
                StartCoroutine(ActivateJumpBoost());
            }
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

    IEnumerator ActivateJumpBoost()
    {
        canUseAbility = false;

        // JUMP
        bikeMovement.rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );

        cooldownRemaining = jumpCooldown;

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
        float maxCooldown = 0f;

        if (UpgradeData.equippedUpgrade == "SpeedBoost")
        {
            upgradeNameText.text = "Speed Boost";
            maxCooldown = speedBoostCooldown;
        }
        else if (UpgradeData.equippedUpgrade == "JumpBoost")
        {
            upgradeNameText.text = "Jump Boost";
            maxCooldown = jumpCooldown;
        }
        else
        {
            upgradeNameText.text = "No Upgrade Equipped";
            cooldownText.text = "";
            UpdateUpgradeImages();

            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = 0f;

            return;
        }

        UpdateUpgradeImages();

        if (canUseAbility)
        {
            cooldownText.text = "Ready";

            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = 0f;
        }
        else
        {
            cooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();

            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = cooldownRemaining / maxCooldown;
        }
    }

    void UpdateUpgradeImages()
    {
        emptyUpgradeImage.SetActive(false);
        speedBoostImage.SetActive(false);
        jumpBoostImage.SetActive(false);

        if (UpgradeData.equippedUpgrade == "SpeedBoost")
        {
            speedBoostImage.SetActive(true);
        }
        else if (UpgradeData.equippedUpgrade == "JumpBoost")
        {
            jumpBoostImage.SetActive(true);
        }
        else
        {
            emptyUpgradeImage.SetActive(true);
        }
    }
}