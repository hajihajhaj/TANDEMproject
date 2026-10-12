
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CustomizationController : MonoBehaviour
{
    [Header("Menu")]
    public CharacterCustomizationMenu customizationMenu;
    public TabController tabController;

    [Header("Customization Scripts")]
    public BikeCustomization bikeCustomization;
    public RoomCustomization roomCustomization;
    public BellCustomization bellCustomization;

    private Gamepad controller;

    void Update()
    {
        // Only respond while the customization menu is open.
        if (customizationMenu == null ||
            customizationMenu.customizationUI == null ||
            !customizationMenu.customizationUI.activeInHierarchy)
        {
            return;
        }

        controller = Gamepad.current;

        if (controller == null)
            return;

        // Circle / O: Close the menu from any tab.
        if (controller.buttonEast.wasPressedThisFrame)
        {
            customizationMenu.CloseMenu();
            return;
        }

        // L1: Previous tab.
        if (controller.leftShoulder.wasPressedThisFrame)
        {
            if (tabController != null)
                tabController.PreviousTab();
        }

        // R1: Next tab.
        if (controller.rightShoulder.wasPressedThisFrame)
        {
            if (tabController != null)
                tabController.NextTab();
        }


        // R2: Test the current bell sound, only on the Bell tab.
        if (tabController != null &&
            tabController.CurrentTab == 3 &&
            controller.rightTrigger.wasPressedThisFrame)
        {
            if (bellCustomization != null)
                bellCustomization.TestBell();
        }


        // D-pad: Browse options on the active tab.
        if (controller.dpad.left.wasPressedThisFrame)
        {
            BrowsePrevious();
        }

        if (controller.dpad.right.wasPressedThisFrame)
        {
            BrowseNext();
        }

        // Cross / X: Select the current option.
        if (controller.buttonSouth.wasPressedThisFrame)
        {
            SelectCurrentOption();
        }
    }

    void BrowsePrevious()
    {
        if (tabController == null)
            return;

        switch (tabController.CurrentTab)
        {
            case 1: // Bike
                if (bikeCustomization != null)
                    bikeCustomization.Back();
                break;

            case 2: // Room
                if (roomCustomization != null)
                    roomCustomization.PreviousRoom();
                break;

            case 3: // Bell
                if (bellCustomization != null)
                    bellCustomization.PreviousBell();
                break;
        }
    }

    void BrowseNext()
    {
        if (tabController == null)
            return;

        switch (tabController.CurrentTab)
        {
            case 1: // Bike
                if (bikeCustomization != null)
                    bikeCustomization.Forward();
                break;

            case 2: // Room
                if (roomCustomization != null)
                    roomCustomization.NextRoom();
                break;

            case 3: // Bell
                if (bellCustomization != null)
                    bellCustomization.NextBell();
                break;
        }
    }

    void SelectCurrentOption()
    {
        if (tabController == null)
            return;

        switch (tabController.CurrentTab)
        {
            case 1: // Bike
                if (bikeCustomization != null)
                    bikeCustomization.Select();
                break;

            case 2: // Room
                if (roomCustomization != null)
                    roomCustomization.SelectRoom();
                break;

            case 3: // Bell
                if (bellCustomization != null)
                    bellCustomization.SelectBell();
                break;

            default:
                // On the Players tab, submit the focused UI button.
                if (EventSystem.current != null)
                {
                    GameObject selected =
                        EventSystem.current.currentSelectedGameObject;

                    if (selected != null)
                    {
                        ExecuteEvents.Execute(
                            selected,
                            new BaseEventData(EventSystem.current),
                            ExecuteEvents.submitHandler
                        );
                    }
                }
                break;
        }
    }
}
