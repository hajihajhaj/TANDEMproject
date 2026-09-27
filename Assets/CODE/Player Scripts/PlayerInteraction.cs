using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Menus")]
    public GameObject shopUI;
    public GameObject achievementsUI;
    public GameObject deliveryUI;
    public CharacterCustomizationMenu customizationMenu;

    [Header("UI Sounds")]
    public AudioSource uiAudioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    [HideInInspector]
    public string currentTrigger = "";

    void Update()
    {
        // OPEN UI
        if ((Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            OpenCurrentUI();

            if (currentTrigger == "CharacterCustomization")
            {
                if (customizationMenu != null)
                {
                    customizationMenu.OpenMenu();
                }
            }
        }

        // CLOSE UI
        if ((Keyboard.current != null && Keyboard.current.backspaceKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            HomeShopUI shop = shopUI.GetComponent<HomeShopUI>();

            // If the "not enough coins" popup is open, close that first
            if (shopUI.activeSelf &&
                shop != null &&
                shop.IsPopupOpen())
            {
                shop.CloseNotEnoughCoinsPopup();
            }
            else
            {
                CloseAllUI();
            }
        }
    }

    void OpenCurrentUI()
    {
        bool openedUI = false;

        if (currentTrigger == "Shop")
        {
            shopUI.SetActive(true);

            HomeShopUI shop =
                shopUI.GetComponent<HomeShopUI>();

            if (shop != null)
            {
                shop.SelectFirstButton();
            }

            openedUI = true;
        }

        if (currentTrigger == "Achievements")
        {
            achievementsUI.SetActive(true);
            openedUI = true;
        }

        if (currentTrigger == "Delivery")
        {
            deliveryUI.SetActive(true);
            openedUI = true;
        }

        // Play open sound
        if (openedUI && uiAudioSource != null && openSound != null)
        {
            uiAudioSource.PlayOneShot(openSound);
        }
    }

    public void CloseAllUI()
    {
        bool hadUIOpen =
            shopUI.activeSelf ||
            achievementsUI.activeSelf ||
            deliveryUI.activeSelf;

        shopUI.SetActive(false);
        achievementsUI.SetActive(false);
        deliveryUI.SetActive(false);

        // Play close sound only if something was actually open
        if (hadUIOpen && uiAudioSource != null && closeSound != null)
        {
            uiAudioSource.PlayOneShot(closeSound);
        }
    }
}