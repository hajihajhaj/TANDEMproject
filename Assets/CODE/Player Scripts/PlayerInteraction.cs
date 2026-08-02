using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Menus")]
    public GameObject shopUI;
    public GameObject achievementsUI;
    public GameObject deliveryUI;
    public CharacterCustomizationMenu customizationMenu;

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
        if (currentTrigger == "Shop")
        {
            shopUI.SetActive(true);

            HomeShopUI shop =
                shopUI.GetComponent<HomeShopUI>();

            if (shop != null)
            {
                shop.SelectFirstButton();
            }
        }

        if (currentTrigger == "Achievements")
        {
            achievementsUI.SetActive(true);
        }

        if (currentTrigger == "Delivery")
        {
            deliveryUI.SetActive(true);
        }
    }

    public void CloseAllUI()
    {
        shopUI.SetActive(false);
        achievementsUI.SetActive(false);
        deliveryUI.SetActive(false);
    }
}