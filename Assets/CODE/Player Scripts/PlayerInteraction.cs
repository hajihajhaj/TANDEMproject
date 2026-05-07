using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Menus")]
    public GameObject shopUI;
    public GameObject achievementsUI;
    public GameObject deliveryUI;

    [HideInInspector]
    public string currentTrigger = "";

    void Update()
    {
        // OPEN UI
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenCurrentUI();
        }

        // CLOSE UI
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseAllUI();
        }
    }

    void OpenCurrentUI()
    {
        if (currentTrigger == "Shop")
        {
            shopUI.SetActive(true);
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