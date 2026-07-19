using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    [Header("Pages")]
    public GameObject mainMenu;
    public GameObject phoneColours;
    public GameObject wallpapers;
    public GameObject notificationSounds;

    [Header("First Selected Buttons")]
    public Button firstMainMenuButton;
    public Button firstPhoneColourButton;
    public Button firstWallpaperButton;
    public Button firstNotificationButton;

    void Start()
    {
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        phoneColours.SetActive(false);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        if (firstMainMenuButton != null)
            EventSystem.current.SetSelectedGameObject(firstMainMenuButton.gameObject);
    }

    public void OpenPhoneColours()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(true);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        if (firstPhoneColourButton != null)
            EventSystem.current.SetSelectedGameObject(firstPhoneColourButton.gameObject);
    }

    public void OpenWallpapers()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(false);
        wallpapers.SetActive(true);
        notificationSounds.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        if (firstWallpaperButton != null)
            EventSystem.current.SetSelectedGameObject(firstWallpaperButton.gameObject);
    }

    public void OpenNotificationSounds()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(false);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        if (firstNotificationButton != null)
            EventSystem.current.SetSelectedGameObject(firstNotificationButton.gameObject);
    }

    public bool IsOnSubPage()
    {
        return phoneColours.activeSelf ||
               wallpapers.activeSelf ||
               notificationSounds.activeSelf;
    }
}