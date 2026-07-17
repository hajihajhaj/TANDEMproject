using UnityEngine;

public class SettingsPageSwitcher : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject phoneColours;
    public GameObject wallpapers;
    public GameObject notificationSounds;

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        phoneColours.SetActive(false);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(false);
    }

    public void OpenPhoneColours()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(true);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(false);
    }

    public void OpenWallpapers()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(false);
        wallpapers.SetActive(true);
        notificationSounds.SetActive(false);
    }

    public void OpenNotificationSounds()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(false);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(true);
    }
}
